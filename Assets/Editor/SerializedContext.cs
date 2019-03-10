using System;
using System.Reflection;


namespace ReflectionView.Editor
{
    public interface SerializedContext
    {
        Type ContextType();
        T GetValue<T>(object parent);
    }


    public class SerializedContextObject : SerializedContext
    {
        private object obj;

        public SerializedContextObject(object obj)
        {
            this.obj = obj;
        }

        public Type ContextType()
        {
            return obj.GetType();
        }

        public T GetValue<T>(object parent)
        {
            return (T)obj;
        }

        //
        public override string ToString()
        {
            return obj.ToString();
        }
    }


    public class SerializedContextField : SerializedContext
    {
        private FieldInfo fieldInfo;


        public SerializedContextField(FieldInfo info)
        {
            this.fieldInfo = info;
        }

        public Type ContextType()
        {
            return fieldInfo.FieldType;
        }

        public T GetValue<T>(object parent)
        {
            return (T)fieldInfo.GetValue(parent);
        }

        //
        public override string ToString()
        {
            return fieldInfo.FieldType.ToString();
        }
    }
}