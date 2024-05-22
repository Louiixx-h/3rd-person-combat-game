using UnityEngine;

namespace CombatGame.Controls
{
    public class MouseController : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                Cursor.visible = true;
            }
            else
            {
                Cursor.visible = false;
            }
        }
    }
}