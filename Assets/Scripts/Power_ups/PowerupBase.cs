using System;
using UnityEngine;

namespace Game.PowerUps
{
    public class PowerupBase : PausableBehaviour
    {
        private Rigidbody2D _rbd2;

        #region Triggers

        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch(collision.tag)
            {
                case "Player":
                    ConsumePowerUp(collision.GetComponent<Player>());
                    Destroy(this.gameObject);                    
                    break;

                case "Enemy":
                    Destroy(this.gameObject);
                    break;
            }
        }       

        #endregion

        private void Awake()
        {
            _rbd2 = GetComponent<Rigidbody2D>();
        }

        #region Powerups actions
        private void ConsumePowerUp(Player player)
        {
            player.Damageable.AddLife(1);
        }
        #endregion

        #region Core

        protected override void OnPause()
        {
            _rbd2.linearVelocity = Vector2.zero;
        }

        protected override void DoUpdate()
        {
            _rbd2.linearVelocity = new Vector2(0, -3f);

            //Out of bounds
            if(transform.position.y <= -20)
            {
                Destroy(this.gameObject);
            }
        }

        #endregion
    }
}
