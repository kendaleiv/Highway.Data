﻿

//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Highway.Data.EntityFramework.Tests.TestContexts
{

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


public partial class TDDAirEntities : DbContext
{
    public TDDAirEntities()
        : base("name=TDDAirEntities")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }

    public DbSet<Member> Members { get; set; }

}

}

