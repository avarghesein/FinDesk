﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findesk.VM.Shared
{
    [Flags]
    public enum Module
    {
        None = 0,
        Users,
        Policy
    }

    [Flags]
    public enum WorkGroupRole
    {
        None = 0,

        Admin = 1,

        UserModuleRead = 2,
        UserModuleReadWrite = 4,

        PolicyModuleRead = 8,
        PolicyModuleReadWrite = 16
    };

    public enum BloodGroup
    {
        Unknown = -1,
        A,
        B,
        AB,
        O
    }

    public enum Gender
    {
        Female = 0,
        Male
    };

    public enum Relation
    {
        Unknown = -1,
        Self,
        Spouse,
        Father,
        Mother,
        FatherInLaw,
        MotherInLaw,
        Child,
        Son,
        Daughter,
    }
};
