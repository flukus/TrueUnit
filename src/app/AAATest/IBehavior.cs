﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AAATest.Mock
{
    public interface IBehavior
    {
    }

    public interface IBehavior<TReturn> : IBehavior
    {
        IBehavior<TReturn> Returns(TReturn returnValue);
        IBehavior<TReturn> Returns<TMock>() where TMock : class, TReturn;
        IBehavior ReturnsSelf();
    }
}
