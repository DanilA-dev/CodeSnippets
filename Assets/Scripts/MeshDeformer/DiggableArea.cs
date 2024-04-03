using PlayerComponents;
using PlayerRelated;
using UnityEngine;

namespace Archeo
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class DiggableArea : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerMovement player))
                ExcavationZone.IsPlayerInExcavationZone = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out ToolEquipment player))
            {
                ExcavationZone.IsPlayerInExcavationZone = false;
                player.Tool.ToolState = PlayerTools.PlayerToolState.Serenity;
            }
        }
    }
}
