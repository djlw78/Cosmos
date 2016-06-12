﻿using System;
using System.Diagnostics.CodeAnalysis;
using Cosmos.Assembler;
using Cosmos.Assembler.x86;
using static XSharp.Compiler.XSRegisters;

namespace XSharp.Compiler
{
  [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
  public static partial class XS
  {
    public static void Label(string labelName)
    {
      new Label(labelName);
    }

    public static void Return()
    {
      new Return();
    }

    public static void InterruptReturn()
    {
      new IRET();
    }

    public static void Set32(string destination, string sourceLabel, bool sourceIsIndirect = false)
    {
      new Mov
      {
        Size = 32,
        DestinationRef = ElementReference.New(destination),
        DestinationIsIndirect = true,
        SourceRef = ElementReference.New(sourceLabel),
        SourceIsIndirect = sourceIsIndirect
      };
    }

    public static void Set(Register destination, string sourceLabel, bool destinationIsIndirect = false, bool sourceIsIndirect = false)
    {
      new Mov
      {
        Size = (byte)destination.Size,
        DestinationReg = destination.RegEnum,
        DestinationIsIndirect = destinationIsIndirect,
        SourceRef = ElementReference.New(sourceLabel),
        SourceIsIndirect = sourceIsIndirect
      };
    }

    public static void Set(Register destination, uint value, bool sourceIsIndirect = false)
    {
      new Mov
      {
        Size = (byte)destination.Size,
        DestinationReg = destination.RegEnum,
        SourceValue = value,
      };
    }

    public static void SetByte(uint address, byte value)
    {
      new Mov { DestinationValue = address, DestinationIsIndirect = true, SourceValue = value };
    }

    public static void Jump(ConditionalTestEnum condition, string label)
    {
      new ConditionalJump { Condition = condition, DestinationLabel = label };
    }

    public static void Jump(string label)
    {
      new Jump { DestinationLabel = label };
    }

    public static void Comment(string comment)
    {
      new Comment(comment);
    }

    public static void Call(string target)
    {
      new Call { DestinationLabel=target };
    }

    public static void Const(string name, string value)
    {
      new LiteralAssemblerCode(name + " equ " + value);
    }

    public static void DataMember(string name, uint value = 0)
    {
      Assembler.CurrentInstance.DataMembers.Add(new DataMember(name, value));
    }

    public static void DataMember(string name, string value)
    {
      Assembler.CurrentInstance.DataMembers.Add(new DataMember(name, "`" + value + "`"));
    }

    public static void DataMember(string name, uint elementCount, string size, string value)
    {
      new LiteralAssemblerCode(name + ": TIMES " + elementCount + " " + size + " " + value);
    }

    public static void RotateRight(Register register, uint bitCount)
    {
      new RotateRight { DestinationReg = register.RegEnum, SourceValue = bitCount, Size = (byte)register.Size };
    }

    public static void RotateLeft(Register register, uint bitCount)
    {
      new RotateLeft { DestinationReg = register.RegEnum, SourceValue = bitCount, Size = (byte)register.Size };
    }

    public static void ShiftRight(Register register, uint bitCount)
    {
      new ShiftRight { DestinationReg = register.RegEnum, SourceValue = bitCount, Size = (byte)register.Size };
    }

    public static void ShiftLeft(Register register, uint bitCount)
    {
      new ShiftLeft { DestinationReg = register.RegEnum, SourceValue = bitCount, Size = (byte)register.Size };
    }

    public static void PushAllGeneralRegisters()
    {
      new Pushad();
    }

    public static void PopAllGeneralRegisters()
    {
      new Popad();
    }

    public static void WriteToPortDX(Register value)
    {
      new OutToDX()
      {
        DestinationReg = value.RegEnum
      };
    }

    public static void ReadFromPortDX(Register value)
    {
      new InFromDX
      {
        DestinationReg = value.RegEnum
      };
    }


  }
}