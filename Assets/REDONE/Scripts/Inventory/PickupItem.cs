using UnityEngine;
using Sirenix.OdinInspector;

namespace CampingTrip
{
    public class PickupItem : MonoBehaviour
    {
        // Serialized and editable from the Unity inspector, editable in other scripts //
        [FoldoutGroup("Variables")] 
        [Title("Item Data", TitleAlignment = TitleAlignments.Centered)][SerializeField] public SoItem item;
    }
}