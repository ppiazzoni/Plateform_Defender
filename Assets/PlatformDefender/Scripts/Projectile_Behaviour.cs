using UnityEngine;

public class Projectile_Behaviour : MonoBehaviour
{
    public float Speed = 4.5f;
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * Speed;
    }

    
    
        private void OnCollisionEnter2D(Collision2D collision)
        {                      
            if (collision.collider.tag != "Player")
            {
               Destroy(collision.gameObject);
            }          
        }      
}
