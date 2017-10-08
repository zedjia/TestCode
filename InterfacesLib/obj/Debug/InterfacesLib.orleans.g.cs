#if !EXCLUDE_CODEGEN
#pragma warning disable 162
#pragma warning disable 219
#pragma warning disable 414
#pragma warning disable 649
#pragma warning disable 693
#pragma warning disable 1591
#pragma warning disable 1998
[assembly: global::System.CodeDom.Compiler.GeneratedCodeAttribute("Orleans-CodeGenerator", "1.5.1.0")]
[assembly: global::Orleans.CodeGeneration.OrleansCodeGenerationTargetAttribute("InterfacesLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
namespace InterfacesLib
{
    using global::Orleans.Async;
    using global::Orleans;
    using global::System.Reflection;

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Orleans-CodeGenerator", "1.5.1.0"), global::System.SerializableAttribute, global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute, global::Orleans.CodeGeneration.GrainReferenceAttribute(typeof (global::InterfacesLib.IChat))]
    internal class OrleansCodeGenChatReference : global::Orleans.Runtime.GrainReference, global::InterfacesLib.IChat
    {
        protected @OrleansCodeGenChatReference(global::Orleans.Runtime.GrainReference @other): base (@other)
        {
        }

        protected @OrleansCodeGenChatReference(global::System.Runtime.Serialization.SerializationInfo @info, global::System.Runtime.Serialization.StreamingContext @context): base (@info, @context)
        {
        }

        protected override global::System.Int32 InterfaceId
        {
            get
            {
                return -846687557;
            }
        }

        protected override global::System.UInt16 InterfaceVersion
        {
            get
            {
                return 1;
            }
        }

        public override global::System.String InterfaceName
        {
            get
            {
                return "global::InterfacesLib.IChat";
            }
        }

        public override global::System.Boolean @IsCompatible(global::System.Int32 @interfaceId)
        {
            return @interfaceId == -846687557;
        }

        protected override global::System.String @GetMethodName(global::System.Int32 @interfaceId, global::System.Int32 @methodId)
        {
            switch (@interfaceId)
            {
                case -846687557:
                    switch (@methodId)
                    {
                        case 886256324:
                            return "ReceiveMessage";
                        default:
                            throw new global::System.NotImplementedException("interfaceId=" + -846687557 + ",methodId=" + @methodId);
                    }

                default:
                    throw new global::System.NotImplementedException("interfaceId=" + @interfaceId);
            }
        }

        public void @ReceiveMessage(global::System.String @message)
        {
            base.@InvokeOneWayMethod(886256324, new global::System.Object[]{@message});
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Orleans-CodeGenerator", "1.5.1.0"), global::Orleans.CodeGeneration.MethodInvokerAttribute(typeof (global::InterfacesLib.IChat), -846687557), global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute]
    internal class OrleansCodeGenChatMethodInvoker : global::Orleans.CodeGeneration.IGrainMethodInvoker
    {
        public global::System.Threading.Tasks.Task<global::System.Object> @Invoke(global::Orleans.Runtime.IAddressable @grain, global::Orleans.CodeGeneration.InvokeMethodRequest @request)
        {
            global::System.Int32 interfaceId = @request.@InterfaceId;
            global::System.Int32 methodId = @request.@MethodId;
            global::System.Object[] arguments = @request.@Arguments;
            if (@grain == null)
                throw new global::System.ArgumentNullException("grain");
            switch (interfaceId)
            {
                case -846687557:
                    switch (methodId)
                    {
                        case 886256324:
                            ((global::InterfacesLib.IChat)@grain).@ReceiveMessage((global::System.String)arguments[0]);
                            return global::Orleans.Async.TaskUtility.@Completed();
                        default:
                            throw new global::System.NotImplementedException("interfaceId=" + -846687557 + ",methodId=" + methodId);
                    }

                default:
                    throw new global::System.NotImplementedException("interfaceId=" + interfaceId);
            }
        }

        public global::System.Int32 InterfaceId
        {
            get
            {
                return -846687557;
            }
        }

        public global::System.UInt16 InterfaceVersion
        {
            get
            {
                return 1;
            }
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Orleans-CodeGenerator", "1.5.1.0"), global::System.SerializableAttribute, global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute, global::Orleans.CodeGeneration.GrainReferenceAttribute(typeof (global::InterfacesLib.IHello))]
    internal class OrleansCodeGenHelloReference : global::Orleans.Runtime.GrainReference, global::InterfacesLib.IHello
    {
        protected @OrleansCodeGenHelloReference(global::Orleans.Runtime.GrainReference @other): base (@other)
        {
        }

        protected @OrleansCodeGenHelloReference(global::System.Runtime.Serialization.SerializationInfo @info, global::System.Runtime.Serialization.StreamingContext @context): base (@info, @context)
        {
        }

        protected override global::System.Int32 InterfaceId
        {
            get
            {
                return 102973649;
            }
        }

        protected override global::System.UInt16 InterfaceVersion
        {
            get
            {
                return 1;
            }
        }

        public override global::System.String InterfaceName
        {
            get
            {
                return "global::InterfacesLib.IHello";
            }
        }

        public override global::System.Boolean @IsCompatible(global::System.Int32 @interfaceId)
        {
            return @interfaceId == 102973649;
        }

        protected override global::System.String @GetMethodName(global::System.Int32 @interfaceId, global::System.Int32 @methodId)
        {
            switch (@interfaceId)
            {
                case 102973649:
                    switch (@methodId)
                    {
                        case -1829957677:
                            return "UnSubscribe";
                        case -1520572651:
                            return "Subscribe";
                        case -1925440376:
                            return "SendUpdateMessage";
                        default:
                            throw new global::System.NotImplementedException("interfaceId=" + 102973649 + ",methodId=" + @methodId);
                    }

                default:
                    throw new global::System.NotImplementedException("interfaceId=" + @interfaceId);
            }
        }

        public global::System.Threading.Tasks.Task @UnSubscribe(global::InterfacesLib.IChat @observer)
        {
            global::Orleans.CodeGeneration.GrainFactoryBase.@CheckGrainObserverParamInternal(@observer);
            return base.@InvokeMethodAsync<global::System.Object>(-1829957677, new global::System.Object[]{@observer is global::Orleans.Grain ? @observer.@AsReference<global::InterfacesLib.IChat>() : @observer});
        }

        public global::System.Threading.Tasks.Task @Subscribe(global::InterfacesLib.IChat @observer)
        {
            global::Orleans.CodeGeneration.GrainFactoryBase.@CheckGrainObserverParamInternal(@observer);
            return base.@InvokeMethodAsync<global::System.Object>(-1520572651, new global::System.Object[]{@observer is global::Orleans.Grain ? @observer.@AsReference<global::InterfacesLib.IChat>() : @observer});
        }

        public global::System.Threading.Tasks.Task @SendUpdateMessage(global::System.String @message)
        {
            return base.@InvokeMethodAsync<global::System.Object>(-1925440376, new global::System.Object[]{@message});
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Orleans-CodeGenerator", "1.5.1.0"), global::Orleans.CodeGeneration.MethodInvokerAttribute(typeof (global::InterfacesLib.IHello), 102973649), global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute]
    internal class OrleansCodeGenHelloMethodInvoker : global::Orleans.CodeGeneration.IGrainMethodInvoker
    {
        public global::System.Threading.Tasks.Task<global::System.Object> @Invoke(global::Orleans.Runtime.IAddressable @grain, global::Orleans.CodeGeneration.InvokeMethodRequest @request)
        {
            global::System.Int32 interfaceId = @request.@InterfaceId;
            global::System.Int32 methodId = @request.@MethodId;
            global::System.Object[] arguments = @request.@Arguments;
            if (@grain == null)
                throw new global::System.ArgumentNullException("grain");
            switch (interfaceId)
            {
                case 102973649:
                    switch (methodId)
                    {
                        case -1829957677:
                            return ((global::InterfacesLib.IHello)@grain).@UnSubscribe((global::InterfacesLib.IChat)arguments[0]).@Box();
                        case -1520572651:
                            return ((global::InterfacesLib.IHello)@grain).@Subscribe((global::InterfacesLib.IChat)arguments[0]).@Box();
                        case -1925440376:
                            return ((global::InterfacesLib.IHello)@grain).@SendUpdateMessage((global::System.String)arguments[0]).@Box();
                        default:
                            throw new global::System.NotImplementedException("interfaceId=" + 102973649 + ",methodId=" + methodId);
                    }

                default:
                    throw new global::System.NotImplementedException("interfaceId=" + interfaceId);
            }
        }

        public global::System.Int32 InterfaceId
        {
            get
            {
                return 102973649;
            }
        }

        public global::System.UInt16 InterfaceVersion
        {
            get
            {
                return 1;
            }
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Orleans-CodeGenerator", "1.5.1.0"), global::System.SerializableAttribute, global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute, global::Orleans.CodeGeneration.GrainReferenceAttribute(typeof (global::InterfacesLib.IUserService))]
    internal class OrleansCodeGenUserServiceReference : global::Orleans.Runtime.GrainReference, global::InterfacesLib.IUserService
    {
        protected @OrleansCodeGenUserServiceReference(global::Orleans.Runtime.GrainReference @other): base (@other)
        {
        }

        protected @OrleansCodeGenUserServiceReference(global::System.Runtime.Serialization.SerializationInfo @info, global::System.Runtime.Serialization.StreamingContext @context): base (@info, @context)
        {
        }

        protected override global::System.Int32 InterfaceId
        {
            get
            {
                return 1609282005;
            }
        }

        protected override global::System.UInt16 InterfaceVersion
        {
            get
            {
                return 1;
            }
        }

        public override global::System.String InterfaceName
        {
            get
            {
                return "global::InterfacesLib.IUserService";
            }
        }

        public override global::System.Boolean @IsCompatible(global::System.Int32 @interfaceId)
        {
            return @interfaceId == 1609282005;
        }

        protected override global::System.String @GetMethodName(global::System.Int32 @interfaceId, global::System.Int32 @methodId)
        {
            switch (@interfaceId)
            {
                case 1609282005:
                    switch (@methodId)
                    {
                        case 1924591127:
                            return "Exist";
                        default:
                            throw new global::System.NotImplementedException("interfaceId=" + 1609282005 + ",methodId=" + @methodId);
                    }

                default:
                    throw new global::System.NotImplementedException("interfaceId=" + @interfaceId);
            }
        }

        public global::System.Threading.Tasks.Task<global::System.Boolean> @Exist(global::System.String @mobileNumber)
        {
            return base.@InvokeMethodAsync<global::System.Boolean>(1924591127, new global::System.Object[]{@mobileNumber});
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Orleans-CodeGenerator", "1.5.1.0"), global::Orleans.CodeGeneration.MethodInvokerAttribute(typeof (global::InterfacesLib.IUserService), 1609282005), global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute]
    internal class OrleansCodeGenUserServiceMethodInvoker : global::Orleans.CodeGeneration.IGrainMethodInvoker
    {
        public global::System.Threading.Tasks.Task<global::System.Object> @Invoke(global::Orleans.Runtime.IAddressable @grain, global::Orleans.CodeGeneration.InvokeMethodRequest @request)
        {
            global::System.Int32 interfaceId = @request.@InterfaceId;
            global::System.Int32 methodId = @request.@MethodId;
            global::System.Object[] arguments = @request.@Arguments;
            if (@grain == null)
                throw new global::System.ArgumentNullException("grain");
            switch (interfaceId)
            {
                case 1609282005:
                    switch (methodId)
                    {
                        case 1924591127:
                            return ((global::InterfacesLib.IUserService)@grain).@Exist((global::System.String)arguments[0]).@Box();
                        default:
                            throw new global::System.NotImplementedException("interfaceId=" + 1609282005 + ",methodId=" + methodId);
                    }

                default:
                    throw new global::System.NotImplementedException("interfaceId=" + interfaceId);
            }
        }

        public global::System.Int32 InterfaceId
        {
            get
            {
                return 1609282005;
            }
        }

        public global::System.UInt16 InterfaceVersion
        {
            get
            {
                return 1;
            }
        }
    }
}
#pragma warning restore 162
#pragma warning restore 219
#pragma warning restore 414
#pragma warning restore 649
#pragma warning restore 693
#pragma warning restore 1591
#pragma warning restore 1998
#endif
