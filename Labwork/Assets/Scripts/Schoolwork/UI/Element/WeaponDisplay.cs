using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles updating the high score display
/// </summary>
namespace Schoolwork.UI.Element
{
    public class WeaponDisplay : UIelement
{
    [Header("References")]
    [Tooltip("The UI Image to display the weapon silouette to")]
    public Image imageToDisplayOn;

    /// <summary>
    /// Description:
    /// Updates the ammo text with the remaining ammo for the currently equipped weapon if ammo is being used
    /// if ammo is not being used for that gun it shows nothing
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    public void DisplayWeaponInformation()
    {
        imageToDisplayOn.sprite = GameManager.Instance.weaponSystem.displaySprite;
        /*Shooter playerShooter = GameManager.Instance.player.GetComponentInChildren<PlayerController>().playerShooter;

        if (ammoText != null && playerShooter.guns[playerShooter.equippedGunIndex].useAmmo)
        {
            ammoText.text = AmmoTracker._instance[playerShooter.guns[playerShooter.equippedGunIndex].ammunitionID].ToString();
            if (ammoPackDisplayImage != null && playerShooter.guns[playerShooter.equippedGunIndex].ammoImage != null)
            {
                ammoPackDisplayImage.color = new Color(255, 255, 255, 255);
                ammoPackDisplayImage.texture = playerShooter.guns[playerShooter.equippedGunIndex].ammoImage.texture;
            }
        }
        else
        {
            ammoText.text = "";
            ammoPackDisplayImage.color = new Color(0,0,0,0);
        }
        if (playerShooter.guns[playerShooter.equippedGunIndex].weaponImage != null && gunDisplayImage != null)
        {
            gunDisplayImage.texture = playerShooter.guns[playerShooter.equippedGunIndex].weaponImage.texture;
        }
        */
    }

    /// <summary>
    /// Description:
    /// Updates the UI element according to this class
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    public override void UpdateUI()
    {
        // This calls the base update UI function from the UIelement class
        base.UpdateUI();

        // The remaining code is only called for this sub-class of UIelement and not others
        DisplayWeaponInformation();
    }
}
}
