using UnityEngine;
using System.Collections.Generic;

public class ImpactEffects : MonoBehaviour
{
    [SerializeField] private GameObject impactStandartEffect;

    [SerializeField] private GameObject impactStoneEffect;
    [SerializeField] private GameObject bulletHoleStoneEffect;

    [SerializeField] private GameObject impactWoodEffect;
    [SerializeField] private GameObject bulletHoleWoodEffect;

    [SerializeField] private GameObject impactMetalEffect;
    [SerializeField] private GameObject bulletHoleMetalEffect;

    // Impact and hole effects
    public void CreateImpactAndHole(Transform collision, ContactPoint contactPoint)
    {
        // Set default impact and hole effects
        GameObject impactEffect = impactStandartEffect;
        GameObject bulletHoleEffect = null;
        
        // Check the object for wood, stone, metal to choose effect style
        if (collision.GetComponent<Renderer>() != null)
        {
            string materialName = MaterialCheck(collision.GetComponent<Renderer>().material.name);
            if (materialName == "stone")
            {
                impactEffect = impactStoneEffect;
                bulletHoleEffect = bulletHoleStoneEffect;
            } else if (materialName == "wood") {
                impactEffect = impactWoodEffect;
                bulletHoleEffect = bulletHoleWoodEffect;
            } else if (materialName == "metal") {
                impactEffect = impactMetalEffect;
                bulletHoleEffect = bulletHoleMetalEffect;
            }
            if (collision.tag == "chain")
                bulletHoleEffect = null;
        }

        // Create an impact effect
        GameObject impact = Instantiate(impactEffect, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));
        Destroy(impact, 1f);

        // Create bullet hole effect (rotate to player and move step from object)
        if (bulletHoleEffect != null)
        {
            Vector3 holePosition = contactPoint.point + contactPoint.normal * 0.0001f;
            GameObject bulletHole = Instantiate(bulletHoleEffect, holePosition, Quaternion.LookRotation(-contactPoint.normal));
            bulletHole.transform.SetParent(collision);
        }

        // Iron chain destroy on hit with no impact and bullet hole effects
        if (collision.tag == "chain" && collision.GetComponent<HingeJoint>() != null)
            Destroy(collision.GetComponent<HingeJoint>());
    }

    // Define what type of material hit the bullet
    public string MaterialCheck(string target)
    {
        // Stone materials
        List<string> stoneMaterialsList = new List<string>
        {"stone wall (Instance)", "stone_wall_2_d (Instance)", "stone_wall_3_d (Instance)", "concrete_1_mat (Instance)",
        "granite_1_d (Instance)"};

        // Wood materials
        List<string> woodMaterialsList = new List<string>
        {"laminate (Instance)", "wooden box (Instance)", "wood_1_d (Instance)", "Chairs_MAT (Instance)",
        "Military target (Instance)", "timber_1_fixed_d (Instance)", "wooden-boards-texture_d (Instance)",
        "BulletDecalWood (Instance)", "paper (Instance)", "_wood_barrel_mat (Instance)", "bag_mat 2 (Instance)",
        "wood_3_d (Instance)", "bark_2_d (Instance)", "trunk_1_d (Instance)", "Grass (Instance)", "Bathroom_Oven_MAT (Instance)",
        "platform_mat_b (Instance)", "Pistol dummy (Instance)", "Machine gun dummy (Instance)", "Shotgun dummy (Instance)",
        "Bow dummy (Instance)"};

        // Metal materials
        List<string> metalMaterialsList = new List<string>
        {"MetalSurface (Instance)", "cont_big_mat_2 (Instance)", "_locker_mat (Instance)", "_mat_lock (Instance)", 
        "cola_can (Instance)", "barrel_mat_1 (Instance)", "Barrel 1 (Instance)", "walls_mat (Instance)", "iron (Instance)",
        "dark_iron (Instance)"};

        if (stoneMaterialsList.Contains(target))
            return "stone";
        else if (woodMaterialsList.Contains(target))
            return "wood";
        else if (metalMaterialsList.Contains(target))
            return "metal";
        else
            return null;
    }
}