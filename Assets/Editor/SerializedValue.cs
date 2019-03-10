using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;


namespace ReflectionView.Editor
{
    public class SerializedValue
    {
        private List<SerializedValue> children = new List<SerializedValue>();
        private SerializedView view;
        private object value;


        public bool HasChildren => children.Count > 0;

        public bool IsExpanded { get; set; } = true;

        public SerializedContext Context { get; private set; }

        public string Name { get; private set; }

        public SerializedValue Parent { get; private set; }


        private int Deep
        {
            get
            {
                SerializedValue obj = this;
                int deep = 0;

                while(obj.Parent != null)
                {
                    deep++;
                    obj = obj.Parent;
                }

                return deep;
            }
        }


        public SerializedValue(SerializedContext context, string name) : this(context, name, null) {}


        public SerializedValue(SerializedContext context, string name, SerializedValue parent)
        {
            this.Parent = parent;
            this.Context = context;
            this.Name = name;

            if (Deep > 10)
            {
                Debug.LogError("too deep");
                return;
            }

            var ctype = context.ContextType();
            //Debug.Log($"{name} : {ctype}");

            view = SerializedViewFactory.ViewForType(ctype);
            if(view == null)
            {
                var fields = ctype.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    SerializedContext fieldContext = new SerializedContextField(field);
                    SerializedValue child = new SerializedValue(fieldContext, field.Name, this);
                    children.Add(child);
                }
            }
        }


        private void Update()
        {
            this.value = Context.GetValue<object>(Parent?.value);

            if(value is Array array)
            {
                children.Clear();

                int intex = 0;
                foreach (var item in array)
                {
                    var child = new SerializedValue(new SerializedContextObject(item), $"[{intex++}]");
                    AddChild(child);
                }
            }

            foreach (var child in children)
            {
                child.Update();
            }
        }


        public void Draw()
        {
            Update();

            view?.Draw(this);
            if(HasChildren)
            {
                this.IsExpanded = EditorGUILayout.Foldout(this.IsExpanded, this.Name);
                if (IsExpanded)
                {
                    EditorGUI.indentLevel++;

                    foreach (var child in children)
                    {
                        child.Draw();
                    }

                    EditorGUI.indentLevel--;
                }
            }
        }


        public T GetValue<T>()
        {
            return Context.GetValue<T>(Parent?.value);
        }


        public void AddChild(SerializedValue child)
        {
            children.Add(child);
        }
    }
}