using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

namespace ReflectionView.Editor
{
    public class SerializedViewFactory
    {
        private static Dictionary<Type, SerializedView> viewMap = new Dictionary<Type, SerializedView>();


        private static SerializedView[] standartViews = {
            new SerializedViewInt(),
            new SerializedViewFloat(),
            new SerializedViewVector2(),
            new SerializedViewVector3(),
            new SerializedViewVector4(),
        };


        static SerializedViewFactory()
        {
            foreach (var view in standartViews)
            {
                viewMap.Add(view.ViewType(), view);
            }
        }


        public static SerializedView ViewForType(Type type)
        {
            if (type.IsArray)
            {
                return new SerializedViewArray();
            }

            viewMap.TryGetValue(type, out SerializedView view);
            return view;
        }
    }


    public interface ISerializedViewType
    {
        Type ViewType();
    }


    public class SerializedView : ISerializedViewType
    {
        public virtual void Draw(SerializedValue value)
        {

        }

        public virtual Type ViewType()
        {
            return typeof(SerializedView);
        }
    }


    public class SerializedView<T> : SerializedView, ISerializedViewType
    {
        public override void Draw(SerializedValue value)
        {

        }


        protected T ToValue(SerializedValue value)
        {
            return value.GetValue<T>();
        }


        public override Type ViewType()
        {
            return typeof(T);
        }
    }


    public class SerializedViewInt : SerializedView<int>
    {
        public override void Draw(SerializedValue value)
        {
            EditorGUILayout.IntField(value.Name, ToValue(value));
        }
    }


    public class SerializedViewFloat : SerializedView<float>
    {
        public override void Draw(SerializedValue value)
        {
            EditorGUILayout.FloatField(value.Name, ToValue(value));
        }
    }


    public class SerializedViewVector4 : SerializedView<Vector4>
    {
        public override void Draw(SerializedValue value)
        {
            EditorGUILayout.Vector4Field(value.Name, ToValue(value));
        }
    }


    public class SerializedViewVector3 : SerializedView<Vector3>
    {
        public override void Draw(SerializedValue value)
        {
            EditorGUILayout.Vector3Field(value.Name, ToValue(value));
        }
    }


    public class SerializedViewVector2 : SerializedView<Vector2>
    {
        public override void Draw(SerializedValue value)
        {
            EditorGUILayout.Vector2Field(value.Name, ToValue(value));
        }
    }


    public class SerializedViewArray : SerializedView<Array>
    {
        public override void Draw(SerializedValue value)
        {
            var array = ToValue(value);
            int length = array != null ? array.Length : 0;
            EditorGUILayout.IntField("Size", length);
        }
    }
}