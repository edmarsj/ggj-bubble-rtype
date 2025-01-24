using System.Collections;
using UnityEngine;

namespace Game.Super_powers
{
    public class Super_powers_base : MonoBehaviour
    {
        public void Use_super_power(string power)
        {
            switch (power)
            {
                case "Laser_barrage":
                    StartCoroutine(Laser_barrage());
                    break;

                case "Explosive_wave":
                    break;
            }
        }

        private IEnumerator Laser_barrage()
        {
            for (int i = 0; i < 5; i++) 
            {
                var laser_barrage_clone = Instantiate(Resources.Load("Prefabs/Bullets/Laser_barrage"), transform.position, Quaternion.identity);

                yield return new WaitForSeconds(1f);
            }
        }

        private void Explosive_wave()
        {

        }
    }
}