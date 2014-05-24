﻿using System.Collections.Generic;

namespace RElmah.Common
{
    public class Operation<T>
    {
        public Operation(IEnumerable<T> targets, OperationType type)
        {
            Targets = targets;
            Type = type;
        }

        public IEnumerable<T> Targets { get; private set; }
        public OperationType Type { get; private set; }
    }
}