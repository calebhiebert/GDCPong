using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts
{
    [ExecuteInEditMode]
    public class SkinningUtility : MonoBehaviour
    {
        // The default skin to apply to the object
        public GameObject DefaultSkin;

        // Whether or not the show the skin in the editor
        public bool PreviewSkin;

        // A variable to hold the currently applied skin
        private GameObject _appliedSkin;

        /// <summary>
        /// Called when the object is spawned
        /// </summary>
        public virtual void Start ()
        {
            ForceClearSkin();
            ApplySkin(DefaultSkin);
        }

        /// <summary>
        /// Called every frame
        /// </summary>
        public virtual void Update()
        {
            if (Application.isEditor && !Application.isPlaying)
            {
                // If preview skin is checked, but the skin is not spawned
                if (PreviewSkin && _appliedSkin == null)
                {
                    ApplySkin(DefaultSkin);
                }

                // If preview skin is not checked, but the skin is spawned
                else if (!PreviewSkin && _appliedSkin != null)
                {
                    ForceClearSkin();
                }
            }
        }

        /// <summary>
        /// Will force delete any spawned skin objects
        /// </summary>
        public void ForceClearSkin()
        {
            if(Application.isEditor && !Application.isPlaying)
            {
                // This means we are in the editor so we have to use DestroyImmidiate instead of Destroy

                List<Transform> childTransforms = GetComponentsInChildren<Transform>().Cast<Transform>().ToList<Transform>();

                foreach (var child in childTransforms)
                {
                    if(child != null && child != transform)
                        DestroyImmediate(child.gameObject);
                }
            }
            else if (Application.isPlaying)
            {
                foreach (var item in gameObject.GetComponentsInChildren<Transform>())
                {
                    if (item != transform)
                    {
                        Destroy(item.gameObject);
                    }
                }
            }

            _appliedSkin = null;
        }

        /// <summary>
        /// Applies a skin to an object
        /// </summary>
        /// <param name="skin">The skin to apply to the object</param>
        /// <returns>The skin object that was spawned</returns>
        public virtual GameObject ApplySkin(GameObject skin)
        {
            // Spawn the skin
            var appliedSkin = Instantiate(skin);

            // Set the skin's parent to the current object, do not keep the world position
            appliedSkin.transform.SetParent(transform, false);

            // Set the name to keep things neat in the unity editor
            appliedSkin.name = skin.name;

            // set the applied skin
            _appliedSkin = appliedSkin;

            return appliedSkin;
        }

        /// <summary>
        /// Return the currently applied skin if any
        /// </summary>
        public GameObject AppliedSkin
        {
            get { return _appliedSkin; }
        }
    }
}
