using UnityEngine;
using UnityEngine.UI;

public class Staff
{
    /*public CraftingMaterial.BaseMaterials baseMaterial;
    public CraftingMaterial.HeadMaterials headMaterial;
    public CraftingMaterial.BaubleMaterials baubleMaterial1;
    public CraftingMaterial.BaubleMaterials baubleMaterial2;
    public CraftingMaterial.HiltBaubleMaterials hiltBaubleMaterial;
    public CraftingMaterial.HiltBaubleMaterials hiltBaubleMaterial2;

    public Staff(CraftingMaterial.BaseMaterials baseMat, CraftingMaterial.HeadMaterials headMat,
                 CraftingMaterial.BaubleMaterials baubleMat1, CraftingMaterial.BaubleMaterials baubleMat2,
                 CraftingMaterial.HiltBaubleMaterials hiltBaubleMat1, CraftingMaterial.HiltBaubleMaterials hiltBaubleMat2)
    {
        baseMaterial = baseMat;
        headMaterial = headMat;
        baubleMaterial1 = baubleMat1;
        baubleMaterial2 = baubleMat2;
        hiltBaubleMaterial = hiltBaubleMat1;
        hiltBaubleMaterial2 = hiltBaubleMat2;
    }*/
    public StaffPart baseMaterial;
    public StaffPart headMaterial;
    public StaffPart baubleMaterial1;
    public StaffPart baubleMaterial2;
    public StaffPart hiltBaubleMaterial;
    public StaffPart hiltBaubleMaterial2;
    //public RawImage icon;
    Sprite icon;
    public Staff(StaffPart baseMat, StaffPart headMat,
                 StaffPart baubleMat1, StaffPart baubleMat2,
                 StaffPart hiltBaubleMat1, StaffPart hiltBaubleMat2)
    {
        baseMaterial = baseMat;
        headMaterial = headMat;
        baubleMaterial1 = baubleMat1;
        baubleMaterial2 = baubleMat2;
        hiltBaubleMaterial = hiltBaubleMat1;
        hiltBaubleMaterial2 = hiltBaubleMat2;
        icon = null;
    }

    /*public void AssignIcon(RawImage newIcon)
    {
        icon = newIcon;
    }*/

    public void AssignIcon(Sprite newIcon)
    {
        icon = newIcon;
    }



}