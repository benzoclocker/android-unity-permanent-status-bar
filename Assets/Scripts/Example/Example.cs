using UnityEngine;
using Utility.Android;

namespace Example
{
    public class ExampleAndroidStatusBar : MonoBehaviour
    {
        private void Awake()
        {
            AndroidStatusBar.EnablePermanentStatusBar(Color.black);
        }
    }
}