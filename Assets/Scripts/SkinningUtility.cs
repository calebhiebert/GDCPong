using UnityEngine;

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
                    Debug.Log("Previewing skin for " + gameObject.name);

                    ApplySkin(DefaultSkin);
                }

                // If preview skin is not checked, but the skin is spawned
                else if (!PreviewSkin && _appliedSkin != null)
                {
                    Debug.Log("Destroying preview for " + gameObject.name);

                    DestroyImmediate(_appliedSkin);
                }
            }
        }

        /// <summary>
        /// Applies a skin to an object
        /// </summary>
        /// <param name="skin">The skin to apply to the object</param>
        /// <returns>The skin object that was spawned</returns>
        public virtual GameObject ApplySkin(GameObject skin)
        {
            Debug.Log("Spawning skin for " + gameObject.name);

            // Spawn the skin
            var appliedSkin = Instantiate(skin);

            // Set the skin's parent to the current object, do not keep the world position
            appliedSkin.transform.SetParent(transform, false);

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
