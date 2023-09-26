using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float maxLifeTime = 1f, timer = 0f;
    public Vector3 targetVector;
    public GameObject meteorPrefab;
    public Vector3 scaleChange = new Vector3(-0.2f, -0.2f, -0.2f);

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * targetVector * Time.deltaTime);
        timer += Time.deltaTime;
        if (timer >= maxLifeTime)
        {
            gameObject.SetActive(false);
            timer = 0f;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            IncreaseScore();
            duplicate(collision.gameObject);
            gameObject.SetActive(false);
        }
    }

    private void IncreaseScore()
    {
        Player.score++;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        GameObject go = GameObject.FindGameObjectWithTag("UI");
        go.GetComponent<Text>().text = "Puntos: " + Player.score;
    }

    public void duplicate(GameObject destroyedMeteor) {
        var positionDestroyed = destroyedMeteor.transform.position;
        var newPos1 = positionDestroyed;
        newPos1.x += 0.5f;
        var newPos2 = positionDestroyed;
        newPos2.x -= 0.5f;
        GameObject meteor1 = Instantiate(meteorPrefab, newPos1, Quaternion.identity);
        GameObject meteor2 = Instantiate(meteorPrefab, newPos2, Quaternion.identity);
        meteor1.transform.localScale += scaleChange;
        meteor2.transform.localScale += scaleChange;
        var rigid1 = meteor1.GetComponent<Rigidbody2D>();
        var rigid2 = meteor2.GetComponent<Rigidbody2D>();
        rigid1.AddForce(transform.right * 100f);
        rigid2.AddForce(transform.right * -100f);
        destroyedMeteor.SetActive(false);
    }
}
