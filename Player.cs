using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public float Velocidade;
    public float ForcaPulo;
    public bool Pulando;
    public bool PuloDuplo;

    private Rigidbody2D rig;
    private Animator anima;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anima = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Andar();
        Pular();
    }

    void Andar()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime * Velocidade;

        if (Input.GetAxis("Horizontal") > 0f)
        {
            anima.SetBool("Run", true);
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }

        else if (Input.GetAxis("Horizontal") < 0f)
        {
            anima.SetBool("Run", true);
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }

        else if (Input.GetAxis("Horizontal") == 0f)
        {
            anima.SetBool("Run", false);
        }

    }

    void Pular()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!Pulando)
            {
                rig.AddForce(new Vector2(0f, ForcaPulo), ForceMode2D.Impulse);
                PuloDuplo = true;
                anima.SetBool("jump", true);
            }

            else
            {
                if (PuloDuplo)
                {
                    rig.AddForce(new Vector2(0f, (ForcaPulo - 4)), ForceMode2D.Impulse);
                    PuloDuplo = false;
                }
            }
        }
    }


    void OnCollisionEnter2D(Collision2D colisao)
    {
        if (colisao.gameObject.layer == 6)
        {
            Pulando = false;
            anima.SetBool("jump", false);
        }

        if (colisao.gameObject.tag == "Monstro")
        {
            Invoke("Reload", 1f);
            Velocidade = 0;
            ForcaPulo = 0;
            anima.SetTrigger("Dead");
            Destroy(gameObject, 1f);
        }

    }

    void Reload()
    {
        SceneManager.LoadScene("Level1");
    }

    void OnCollisionExit2D(Collision2D colisao)
    {
        if (colisao.gameObject.layer == 6)
        {
            Pulando = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monstro")
        {
            rig.velocity = Vector2.zero;
            rig.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
            collision.gameObject.GetComponent<SpriteRenderer>().flipY = true;
            collision.gameObject.GetComponent<Enemy>().enabled = false;
            collision.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            collision.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            Destroy(collision.gameObject, 1f);
        }
    }
}

