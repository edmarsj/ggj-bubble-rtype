using UnityEngine;

namespace Game.Super_powers
{
    public class Laser_barrage_base : PausableBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private int _damage;
        [SerializeField] private float _maxLife_time;

        [Space(10)]
        [SerializeField] private bool _right;

        private Rigidbody2D _rb2d;
        private float _lifeTime;

        #region Triggers

        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (collision.tag) 
            {
                case "Enemy":
                    collision.gameObject.GetComponent<Damageable>().TakeDamage(_damage);
                    break;

                case "Player":
                    collision.gameObject.GetComponent<Damageable>().TakeDamage(_damage);

                    Destroy(this.gameObject);
                    break;
            }
        }

        #endregion

        private void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();
        }

        #region Core

        protected override void DoUpdate()
        {
            //Set
            if (_right) { _rb2d.linearVelocity = new Vector2(_speed, 0); }
            else { _rb2d.linearVelocity = new Vector2(-_speed, 0); }

            _lifeTime += Time.deltaTime;

            if( _lifeTime >= _maxLife_time)
            {
                Destroy(this.gameObject);
            }
        }

        protected override void OnPause()
        {
            //Set
            _rb2d.linearVelocity = Vector2.zero;
        }

        #endregion
    }
}
