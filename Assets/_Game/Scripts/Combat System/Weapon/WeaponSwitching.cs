using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    [SerializeField] private int _selectedWeapon=0;
    [SerializeField] private PlayerEarthController _playerEarthController;
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


        if(!_playerEarthController.IsEarth())
        {
            WeaponScroll();
        }
        else
        {
            _selectedWeapon = 0;
        }

        if (_previousSelectedWeapon != _selectedWeapon)
        {
            SelectWeapon();
        }
    }

    private void WeaponScroll()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (_selectedWeapon >= transform.childCount - 1)
                _selectedWeapon = 0;
            else
                _selectedWeapon++;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (_selectedWeapon <= 0)
                _selectedWeapon = transform.childCount - 1;
            else
                _selectedWeapon--;
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

    /// <summary>
    /// Get active weapon
    /// </summary>
    /// <returns>0 for empty, 1 for gun and 2 for knife</returns>
    public int GetWeapon()
    {
        return _selectedWeapon;
    }

}
