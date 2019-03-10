using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReflectionView
{
    public class SomeData
    {
        public int val;
        public Vector3 vec;

        //public Vector3[] vec = new Vector3[4];
    }

    [ExecuteInEditMode]
    public class Sample : MonoBehaviour
    {
        //[SerializeField] private float Foo;
        //private SomeData someData;
        public SomeData[] array = new SomeData[5];


        private void Update()
        {
            //Foo += 0.01f;
        }
    }
}