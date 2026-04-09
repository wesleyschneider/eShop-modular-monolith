global using System;
global using System.Threading;
global using System.Threading.Tasks;
global using MediatR;
global using Microsoft.VisualStudio.TestTools.UnitTesting;
global using NSubstitute;

[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]
