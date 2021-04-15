using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedCustomizableSystem
{
    public static class CharacterGenerator
    {
        public static void Generate(CharacterCustomization cc)
        {
            int CheckExclude(int hair, int beard, int hat, int accessory, int shirt, int pants, int shoes, List<ExcludeIndexes> excludeIndexes)
            {
                int out_index = 0;
                if (excludeIndexes.Count == 0)
                {
                    out_index = -1;
                }
                else
                {
                    foreach (var e in excludeIndexes)
                    {
                        if (e.item == ExcludeItem.Hair && hair == e.index)
                        {
                            out_index = -1;
                            break;
                        }

                        if (e.item == ExcludeItem.Beard && beard == e.index)
                        {
                            out_index = -1;
                            break;
                        }

                        if (e.item == ExcludeItem.Hat && hat == e.index)
                        {
                            out_index = -1;
                            break;
                        }
                        if (e.item == ExcludeItem.Accessory && accessory == e.index)
                        {
                            out_index = -1;
                            break;
                        }

                        if (e.item == ExcludeItem.Shirt && shirt == e.index)
                        {
                            out_index = -1;
                            break;
                        }

                        if (e.item == ExcludeItem.Pants && pants == e.index)
                        {
                            out_index = -1;
                            break;
                        }
                        if (e.item == ExcludeItem.Shoes && shoes == e.index)
                        {
                            out_index = -1;
                            break;
                        }
                    }
                }
                return out_index;
            }
            var generatorSettings = cc.CharacterGeneratorSettings;

            int hairIndex = generatorSettings.hair.GetRandom(cc.hairPresets.Count);
            int beardIndex = generatorSettings.beard.GetRandom(cc.beardPresets.Count);
            int hatIndex = generatorSettings.hat.GetRandom(cc.hatsPresets.Count);
            int accessoryIndex = generatorSettings.accessory.GetRandom(cc.accessoryPresets.Count);
            int shirtIndex = generatorSettings.shirt.GetRandom(cc.shirtsPresets.Count);
            int pantsIndex = generatorSettings.pants.GetRandom(cc.pantsPresets.Count);
            int shoesIndex = generatorSettings.shoes.GetRandom(cc.shoesPresets.Count);
            float headSize = generatorSettings.headSize.GetRandom();
            float headOffset = generatorSettings.headOffset.GetRandom();
            float height = generatorSettings.height.GetRandom();

            foreach (var exclude in generatorSettings.excludes)
            {
                var r = CheckExclude(hairIndex, beardIndex, hatIndex, accessoryIndex, shirtIndex, pantsIndex, shoesIndex, exclude.exclude);
                if (r == -1)
                {
                    if (exclude.ExcludeItem == ExcludeItem.Hair && hairIndex == exclude.targetIndex)
                    {
                        hairIndex = -1;
                    }
                    if (exclude.ExcludeItem == ExcludeItem.Beard && beardIndex == exclude.targetIndex)
                    {
                        beardIndex = -1;
                    }
                    if (exclude.ExcludeItem == ExcludeItem.Hat && hatIndex == exclude.targetIndex)
                    {
                        hatIndex = -1;
                    }
                    if (exclude.ExcludeItem == ExcludeItem.Accessory && accessoryIndex == exclude.targetIndex)
                    {
                        accessoryIndex = -1;
                    }
                    if (exclude.ExcludeItem == ExcludeItem.Shirt && shirtIndex == exclude.targetIndex)
                    {
                        shirtIndex = -1;
                    }
                    if (exclude.ExcludeItem == ExcludeItem.Pants && pantsIndex == exclude.targetIndex)
                    {
                        pantsIndex = -1;
                    }
                    if (exclude.ExcludeItem == ExcludeItem.Shoes && shoesIndex == exclude.targetIndex)
                    {
                        shoesIndex = -1;
                    }
                }
            }

            cc.SetHairByIndex(hairIndex);
            cc.SetBeardByIndex(beardIndex);

            cc.SetHeadSize(headSize);
            cc.SetHeadOffset(headOffset);
            cc.SetHeight(height);

            cc.SetBodyShape(BodyShapeType.Fat, generatorSettings.fat.GetRandom());
            cc.SetBodyShape(BodyShapeType.Muscles, generatorSettings.muscles.GetRandom());
            cc.SetBodyShape(BodyShapeType.Slimness, generatorSettings.thin.GetRandom());
            cc.SetBodyShape(BodyShapeType.Thin, generatorSettings.thin.GetRandom());

            cc.SetElementByIndex(ClothesPartType.Accessory, accessoryIndex);
            cc.SetElementByIndex(ClothesPartType.Shirt, shirtIndex);
            cc.SetElementByIndex(ClothesPartType.Pants, pantsIndex);
            cc.SetElementByIndex(ClothesPartType.Shoes, shoesIndex);
            cc.SetElementByIndex(ClothesPartType.Hat, hatIndex);

            cc.SetBodyColor(BodyColorPart.Skin, generatorSettings.skinColors.GetRandom());
            cc.SetBodyColor(BodyColorPart.Hair, generatorSettings.hairColors.GetRandom());
            cc.SetBodyColor(BodyColorPart.Eye, generatorSettings.eyeColors.GetRandom());
            Dictionary<FaceShapeType, float> facialBlendshapes = new Dictionary<FaceShapeType, float>();

            foreach (var fbs in generatorSettings.facialBlendshapes)
            {
                if (Enum.TryParse(fbs.name, out FaceShapeType faceShapeType))
                    facialBlendshapes.Add(faceShapeType, fbs.GetRandom());
            }
            cc.SetFaceShapeByArray(facialBlendshapes);
        }
    }
}