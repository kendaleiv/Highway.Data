﻿using System.Linq;
using Highway.Data.Tests.TestDomain;

namespace Highway.Data.EntityFramework.Tests.TestQueries
{
    public class StoredProcTestQuery : AdvancedQuery<Foo>
    {
        public StoredProcTestQuery()
        {
            ContextQuery = context =>
            {
                var parameters = new FooName("Devlin");
                var list = context.CallStoredProc(new GetFoos(), parameters).ToList<Foo>();
                list.ForEach(x => x.AttachEntity(context));
                return list.AsQueryable();
            };
        }

        private class FooName
        {
            public FooName(string name)
            {
                Name = name;
            }

            [StoredProcedureAttributes.Name("Name")]
            public string Name { get; set; }
        }

        private class GetFoos : StoredProc<FooName>
        {
            public GetFoos()
            {
                HasName("GetFoos").HasOwner("dbo").ReturnsTypes(typeof (Foo));
            }
        }
    }
}