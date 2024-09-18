using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private GameObject signObject = null;
    #endregion

    #region PRIVATE_FIELDS
    #endregion

    #region UNITY_CALLS
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<InteractableObject>() != null)
        {
            signObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<InteractableObject>() != null)
        {
            signObject.SetActive(false);
        }
    }
    #endregion
}