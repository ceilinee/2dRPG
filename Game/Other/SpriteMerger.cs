using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMerger : MonoBehaviour {

    // public GameObject instance;// assumes you've dragged a reference into this
    // // public Animal.DictionaryOfStringAndColor newColoring;// a transform with a bunch of SpriteRenderers you want to merge
    // public Animals curAnimals;
    // public GenericAnimal animal;
    // public Color avoid;
    // // Use this for initialization
    // void Start () {
    //     Create(new Vector2Int(64, 64), curAnimals);
    // }
    // public Sprite Create(Vector2Int size, Animals curAnimals) {
    //     Animal.DictionaryOfStringAndColor newColoring = curAnimals.animalArray[0].coloring;
    //     var targetTexture = new Texture2D(size.x, size.y, TextureFormat.RGBA32, false, false);
    //     targetTexture.filterMode = FilterMode.Point;
    //     var targetPixels = targetTexture.GetPixels();
    //     // instance.GetComponent<GenericAnimal>().animalName = a.animalName;
    //     foreach (KeyValuePair<string, Color> kvp in newColoring)
    //     {
    //       Debug.Log(kvp.Key + kvp.Value);
    //       Transform trans = instance.transform;
    //       if(kvp.Key == "body"){
    //         Debug.Log(newColoring["body"]);
    //         Transform childTrans = trans.Find("BodySocket");
    //         SpriteRenderer sprite = childTrans.gameObject.GetComponent<SpriteRenderer>();
    //         sprite.color = newColoring["body"];
    //         targetPixels = Merge(targetPixels, sprite);
    //       }
    //       if(kvp.Key == "tail"){
    //         Transform childTrans = trans.Find("TailSocket");
    //         Debug.Log(newColoring["tail"]);
    //         SpriteRenderer sprite = childTrans.gameObject.GetComponent<SpriteRenderer>();
    //         sprite.color = newColoring["tail"];
    //         targetPixels = Merge(targetPixels, sprite);
    //       }
    //       // if(kvp.Key == "star"){
    //       //   Transform childTrans = trans.Find("StarSocket");
    //       //   Debug.Log(newColoring["star"]);
    //       //   SpriteRenderer sprite = childTrans.gameObject.GetComponent<SpriteRenderer>();
    //       //   sprite.color = newColoring["star"];
    //       //   targetPixels = Merge(targetPixels, sprite);
    //       // }
    //       if(kvp.Key == "dots"){
    //         Transform childTrans = trans.Find("DotsSocket");
    //         Debug.Log(newColoring["dots"]);
    //         SpriteRenderer sprite = childTrans.gameObject.GetComponent<SpriteRenderer>();
    //         sprite.color = newColoring["dots"];
    //         targetPixels = Merge(targetPixels, sprite);
    //       }
    //       // if(kvp.Key == "eyes"){
    //       //   Transform childTrans = trans.Find("EyesSocket");
    //       //   Debug.Log(newColoring["eyes"]);
    //       //   SpriteRenderer sprite = childTrans.gameObject.GetComponent<SpriteRenderer>();
    //       //   sprite.color = newColoring["eyes"];
    //       //   targetPixels = Merge(targetPixels, sprite);
    //       // }
    //   }
    //   targetTexture.SetPixels(targetPixels);
    //   targetTexture.Apply(false, true);// read/write is disabled in 2nd param to free up memory
    //   return Sprite.Create(targetTexture, new Rect(new Vector2(), size), new Vector2(), 1, 0, SpriteMeshType.FullRect);
    // }
    // public Color[] Merge(Color[] result, SpriteRenderer curSprite){
    //   var sourcePixels = curSprite.sprite.texture.GetPixels();
    //   for(int j = 0; j < sourcePixels.Length; j++) {
    //       var source = sourcePixels[j];
    //       if(j > 0 && j < result.Length) {
    //           var target = result[j];
    //           if(source.a > 0) {
    //               result[j] = sourcePixels[j];
    //           }
    //       }
    //   }
    //   return result;
    // }
}

// public class SpriteMerger : MonoBehaviour {
//
//     public SpriteRenderer spriteRenderer;// assumes you've dragged a reference into this
//     public SpriteRenderer[] mergeInput;// a transform with a bunch of SpriteRenderers you want to merge
//     public GenericAnimal animal;
//     // Use this for initialization
//     void Start () {
//         animal.animalSprite = Create(new Vector2Int(64, 64), mergeInput);
//     }
//     public Sprite Create(Vector2Int size, SpriteRenderer[] input) {
//         var spriteRenderers = input;
//         if(spriteRenderers.Length == 0) {
//             Debug.Log("No SpriteRenderers found");
//             return null;
//         }
//
//         var targetTexture = new Texture2D(size.x, size.y, TextureFormat.RGBA32, false, false);
//         // var targetTexture = spriteRenderer.sprite.texture;
//         // var cloneTexture = CopyTexture(targetTexture, new Texture2D(64, 64, TextureFormat.RGBA32, false, false);)
//         targetTexture.filterMode = FilterMode.Point;
//         var targetPixels = targetTexture.GetPixels();
//         // for(int i = 0; i < targetPixels.Length; i++) targetPixels[i] = Color.clear;// default pixels are not set
//
//         for(int i = 0; i < spriteRenderers.Length; i++) {
//             // var position = (Vector2)sr.transform.localPosition - sr.sprite.pivot;
//             // Debug.Log(position);
//
//             // var p = new Vector2Int(0, 0);
//             // var sourceWidth = sr.sprite.texture.width;
//             // if read/write is not enabled on texture (under Advanced) then this next line throws an error
//             // no way to check this without Try/Catch :(
//             var sourcePixels = spriteRenderers[i].sprite.texture.GetPixels();
//             for(int j = 0; j < sourcePixels.Length; j++) {
//                 var source = sourcePixels[j];
//                 var index = j;
//                 if(index > 0 && index < targetPixels.Length) {
//                     var target = targetPixels[index];
//                     if(source.a > 0) {
//                         targetPixels[index] = sourcePixels[index];
//                     }
//                 }
//             }
//         }
//
//         targetTexture.SetPixels(targetPixels);
//         targetTexture.Apply(false, true);// read/write is disabled in 2nd param to free up memory
//         return Sprite.Create(targetTexture, new Rect(new Vector2(), size), new Vector2(), 1, 0, SpriteMeshType.FullRect);
//     }
// }
