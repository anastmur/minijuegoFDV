using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float rotationSpeed = 120f;
    public float movementSpeed = 100f;

    public GameObject gun, bulletPrefab;

    private Rigidbody2D _rigid; // barra baja en una variable privada por convención

    public static int score = 0;

    private float xLimit = 9.5f, yLimit = 5.5f;

    public int bulletsToPool = 100;
    public List<GameObject> pooledBullets;

    // Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>(); // asigna a _rigid el RigidBody2D del componente que queremos manipular
        pooledBullets = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < bulletsToPool ;i++)
        {
            tmp = Instantiate(bulletPrefab);
            tmp.SetActive(false);
            pooledBullets.Add(tmp);
        }

    }

    // Update is called once per frame
    void Update()
    {

        // PAUSA
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject pauseText = GameObject.FindGameObjectWithTag("Pause");
            var pauseTextPos = pauseText.transform.position;
            if(Time.timeScale == 1f)
            {
                pauseTextPos.x = 100f;
                Time.timeScale = 0f;
            }
            else if(Time.timeScale == 0f)
            {
                pauseTextPos.x = 10000f;
                Time.timeScale = 1f;
            }
            pauseText.transform.position = pauseTextPos;
        }

        // ESPACIO INFINITO
        var pos = transform.position;
        if(Mathf.Abs(pos.x) > xLimit)
        {
            pos.x = -pos.x;
        }
        else if(Mathf.Abs(pos.y) > yLimit)
        {
            pos.y = -pos.y;
        }
        transform.position = pos;

         // TRASLACIÓN
        float thrust = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime; // La clase Input permite detectar los botones pulsados
                                                                                  // (Lee notas *1)`
                                                                                  // (Para deltaTime lee notas *2)
        Vector2 movementDirection = transform.right; // El objeto siempre se moverá hacía la derecha, al rotar su "derecha" cambiará
        _rigid.AddForce(thrust * movementDirection); // Usa un Vector 2D para determinar a donde se quiere empujar el objeto

        // ROTACIÓN
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;        
        transform.Rotate(Vector3.forward, -rotation);

        var verticalCameraSize   = Camera.main.orthographicSize;
        var horizontalCameraSize = (verticalCameraSize * Screen.width / Screen.height)/2;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet = GetPooledObject(); 
            if (bullet != null) {
                bullet.transform.position = gun.transform.position;
                bullet.transform.rotation = Quaternion.identity;
                bullet.SetActive(true);
                Bullet balaScript = bullet.GetComponent<Bullet>();
                balaScript.targetVector = transform.right;
            }
        }

    }

    private GameObject GetPooledObject()
    {
        for(int i = 0; i < bulletsToPool; i++)
        {
            if(!pooledBullets[i].activeInHierarchy)
            {
                return pooledBullets[i];
            }
        }
        return null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.CompareTag("Enemy"))
        {
            score = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
