using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    [SerializeField] private int _selectedWeapon=0;
    private int _previousSelectedWeapon;

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        _previousSelectedWeapon = _selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel")>0f)
        {
            if (_selectedWeapon >= transform.childCount - 1)
                _selectedWeapon = 0;
            else
                _selectedWeapon++;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (_selectedWeapon <=0)
                _selectedWeapon = transform.childCount - 1;
            else
                _selectedWeapon--;
        }

        if(_previousSelectedWeapon!=_selectedWeapon)
        {
            SelectWeapon();
        }
    }

    private void SelectWeapon()
    {
        int i = 0;

        foreach (Transform weapon in transform)
        {
            if (i == _selectedWeapon)

                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);

            i++;
        }
    }

}
