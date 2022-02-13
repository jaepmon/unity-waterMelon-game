using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{

    public bool isDrag;

    public bool isChange;

    public int level;

    float gameOverTime;

    Rigidbody2D rb;

    CircleCollider2D cc;

    Animator anim;

    SpriteRenderer sr;

    GameManager gm;
    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        anim.SetInteger("Level", level);
    }

    void OnDisable()
    {
        isDrag = false;

        isChange = false;

        level = 0;

        rb.simulated = false;

        rb.velocity = Vector2.zero;

        rb.angularVelocity = 0;

        cc.enabled = true;
    }
    private void Update()
    {
        if (isDrag)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            gameObject.transform.position = new Vector3(Mathf.Clamp(mousePos.x, -4f+transform.localScale.x / 2f, 4.3f - transform.localScale.x / 2f), gameObject.transform.position.y, 0f);
        }
    }
    public void objectMove()
    {
        isDrag = true;
    }
    public void objectDrop()
    {
        isDrag = false;

        rb.simulated = true;
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "cube")
        {
            SpawnObject other = collision.gameObject.GetComponent<SpawnObject>();

            if(level == other.level && !isChange &&!other.isChange && level < 7)
            {
                float mX = transform.position.x;
                float mY = transform.position.y;
                float otherX = other.transform.position.x;
                float otherY = other.transform.position.y;

                if(mY < otherY || (mY <= otherY && mX <= otherX))
                {
                    other.Hide(transform.position);
                    
                    LevelUp();

                    ObjectPool.instance.mint.Enqueue(other.gameObject);
                }
            }
        }
    }
    public void Hide(Vector3 target)
    {
        isChange = true;

        rb.simulated = false;

        cc.enabled = false;

        StartCoroutine(HideCoroutine(target));
    }
    IEnumerator HideCoroutine(Vector3 target)
    {
        int isCount = 0;

        while (isCount < 15)
        {
            isCount++;

            transform.position = Vector3.Lerp(transform.position, target, 0.5f);

            yield return null;
        }
        gm.currentScore += (int)Mathf.Pow(3, level);

        isChange = false;

        gameObject.SetActive(false);
    }
    void LevelUp()
    {
        isChange = true;

        StartCoroutine(LevelUpCoroutine());
    }
    IEnumerator LevelUpCoroutine()
    {
        yield return new WaitForSeconds(0.2f);

        anim.SetInteger("Level", level + 1);

        yield return new WaitForSeconds(0.3f);

        level++;

        gm.maxLevel = Mathf.Max(level, gm.maxLevel);

        isChange = false;
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Finish")
        {
            gameOverTime += Time.deltaTime;

            if(gameOverTime > 2)
            {
                sr.color = Color.red;
            }
            if (gameOverTime > 5)
            {
                gm.GameOver();

            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Finish")
        {
            gameOverTime = 0;

            sr.color = Color.white;
        }
    }
}
