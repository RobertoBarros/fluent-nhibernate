using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Instances
{
    public class VersionInstance : VersionInspector, IVersionInstance
    {
        private readonly VersionMapping mapping;
        private bool nextBool = true;

        public VersionInstance(VersionMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public new IAccessInstance Access
        {
            get
            {
                return new AccessInstance(value =>
                {
                    if (!mapping.IsSpecified("Access"))
                        mapping.Access = value;
                });
            }
        }

        public new IGeneratedInstance Generated
        {
            get
            {
                return new GeneratedInstance(value =>
                {
                    if (!mapping.IsSpecified("Generated"))
                        mapping.Generated = value;
                });
            }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public IVersionInstance Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        public void Column(string columnName)
        {
            if (mapping.Columns.UserDefined.Count() > 0)
                return;

            var originalColumn = mapping.Columns.FirstOrDefault();
            var column = originalColumn == null ? new ColumnMapping() : ColumnMapping.BaseOn(originalColumn);

            column.Name = columnName;

            mapping.ClearColumns();
            mapping.AddColumn(column);
        }

        public new void UnsavedValue(string unsavedValue)
        {
            if (!mapping.IsSpecified("UnsavedValue"))
                mapping.UnsavedValue = unsavedValue;
        }

        public new void Length(int length)
        {
            if (mapping.Columns.First().IsSpecified("Length"))
                return;

            foreach (var column in mapping.Columns)
                column.Length = length;
        }

        public new void Precision(int precision)
        {
            if (mapping.Columns.First().IsSpecified("Precision"))
                return;

            foreach (var column in mapping.Columns)
                column.Precision = precision;
        }

        public new void Scale(int scale)
        {
            if (mapping.Columns.First().IsSpecified("Scale"))
                return;

            foreach (var column in mapping.Columns)
                column.Scale = scale;
        }

        public new void Nullable()
        {
            if (!mapping.Columns.First().IsSpecified("NotNull"))
                foreach (var column in mapping.Columns)
                    column.NotNull = !nextBool;

            nextBool = true;
        }

        public new void Unique()
        {
            if (!mapping.Columns.First().IsSpecified("Unique"))
                foreach (var column in mapping.Columns)
                    column.Unique = nextBool;

            nextBool = true;
        }

        public new void UniqueKey(string columns)
        {
            if (mapping.Columns.First().IsSpecified("UniqueKey"))
                return;

            foreach (var column in mapping.Columns)
                column.UniqueKey = columns;
        }

        public void CustomSqlType(string sqlType)
        {
            if (mapping.Columns.First().IsSpecified("SqlType"))
                return;

            foreach (var column in mapping.Columns)
                column.SqlType = sqlType;
        }

        public new void Index(string index)
        {
            if (mapping.Columns.First().IsSpecified("Index"))
                return;

            foreach (var column in mapping.Columns)
                column.Index = index;
        }

        public new void Check(string constraint)
        {
            if (mapping.Columns.First().IsSpecified("Check"))
                return;

            foreach (var column in mapping.Columns)
                column.Check = constraint;
        }

        public new void Default(object value)
        {
            if (mapping.Columns.First().IsSpecified("Default"))
                return;

            foreach (var column in mapping.Columns)
                column.Default = value.ToString();
        }

        public void CustomType(string type)
        {
            if (!mapping.IsSpecified("Type"))
                mapping.Type = new TypeReference(type);
        }

        public void CustomType(Type type)
        {
            if (!mapping.IsSpecified("Type"))
                mapping.Type = new TypeReference(type);
        }

        public void CustomType<T>()
        {
            CustomType(typeof(T));
        }
    }
}