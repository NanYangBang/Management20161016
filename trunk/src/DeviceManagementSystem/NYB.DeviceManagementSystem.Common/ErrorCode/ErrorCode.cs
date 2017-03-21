using System;

namespace NYB.DeviceManagementSystem.Common
{
    /// <summary>
    /// 记录所有Web服务的错误信息
    /// 示例：10123
    /// 1系统错误；2为服务级错误
    /// 01服务模块代码
    ///     00 通用错误
    ///		01 用户管理
    ///		02 项目管理
    /// 23具体错误代码
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// 未知错误
        /// </summary>
        [Description("未知错误")]
        UnknownError = 1,

        /// <summary>
        /// 系统错误
        /// </summary>
        [Description("系统错误")]
        SystemError = 10001,

        /// <summary>
        /// 参数错误
        /// </summary>
        [Description("参数错误")]
        ParameterError = 20001,

        /// <summary>
        /// 数据库错误
        /// </summary>
        [Description("数据库错误")]
        DbError = 20002,

        /// <summary>
        /// 没有查询到数据
        /// </summary>
        [Description("没有查询到数据")]
        NoData = 20003,

        /// <summary>
        /// 更新数据库失败
        /// </summary>
        [Description("更新数据库失败")]
        SaveDbChangesFailed = 20004,

        /// <summary>
        /// 保存文件失败
        /// </summary>
        [Description("保存文件失败")]
        SaveFileFailed = 20005,

        /// <summary>
        /// {0}数据不存在，或已被删除
        /// </summary>
        [Description("{0}数据不存在，或已被删除")]
        DataNoExistWithParam = 20006,

        /// <summary>
        /// 操作的数据不存在，或已被删除
        /// </summary>
        [Description("操作的数据不存在，或已被删除")]
        DataNoExist = 20007,

        /// <summary>
        /// 用户信息不正确
        /// </summary>
        [DescriptionAttribute("用户信息不正确")]
        UserInfoError = 20008,

        /// <summary>
        /// 数据重复
        /// </summary>
        [DescriptionAttribute("数据重复")]
        DataRepeat = 20009,

        [DescriptionAttribute("添加用户失败")]
        AddUserFault = 20101,

        [DescriptionAttribute("用户或密码错误")]
        UserNameOrPasswordWrong = 20102,

        [DescriptionAttribute("用户或密码错误")]
        ProjectNotExist = 20103,

        [DescriptionAttribute("设备类型下存在关联设备，不可删除")]
        DeviceTypeConatinDevice = 20104,

        [DescriptionAttribute("供应商下存在关联设备，不可删除")]
        SupplierConatinDevice = 20105,

        [DescriptionAttribute("生产厂商下存在关联设备，不可删除")]
        ManufactureConatinDevice = 20106,

        [DescriptionAttribute("生产厂商下存在关联设备，不可删除")]
        DeviceNotExist = 20107,

        [DescriptionAttribute("文件中没有数据")]
        FileContainNoData = 20108,

        /// <summary>
        /// 用户不存在
        /// </summary>
        [DescriptionAttribute("用户不存在")]
        UserNotExist = 20109,

        /// <summary>
        /// 设备编号不允许重复
        /// </summary>
        [DescriptionAttribute("设备编号不允许重复")]
        DeviceNumIsExist = 20110,

        /// <summary>
        /// 设备类型名称不允许重复
        /// </summary>
        [DescriptionAttribute("设备类型名称不允许重复")]
        DeviceTypeNameIsExist = 20111,

        /// <summary>
        /// 生产厂商名称不允许重复
        /// </summary>
        [DescriptionAttribute("生产厂商名称不允许重复")]
        ManufacturerNameIsExist = 20112,

        /// <summary>
        /// 供应商名称不允许重复
        /// </summary>
        [DescriptionAttribute("供应商名称不允许重复")]
        SupplierNameIsExist = 20113,

        /// <summary>
        /// 设备类型不存在
        /// </summary>
        [DescriptionAttribute("设备类型不存在")]
        DeviceTypeNotExist = 20114,

        /// <summary>
        /// 生产厂商不存在
        /// </summary>
        [DescriptionAttribute("生产厂商不存在")]
        ManufacturerNotExist = 20115,

        /// <summary>
        /// 供应商不存在
        /// </summary>
        [DescriptionAttribute("供应商不存在")]
        SupplierNotExist = 20116,

        /// <summary>
        /// 项目名称不允许重复
        /// </summary>
        [DescriptionAttribute("项目名称不允许重复")]
        ProjectNameIsExist = 20117,

        /// <summary>
        /// 登录名已存在
        /// </summary>
        [DescriptionAttribute("登录名已存在")]
        LoginNameIsExist = 20118,

        /// <summary>
        /// 修改密码失败
        /// </summary>
        [DescriptionAttribute("修改密码失败")]
        ChangePasswordError = 20119,

        /// <summary>
        /// 客户名称不允许重复
        /// </summary>
        [DescriptionAttribute("客户名称不允许重复")]
        OrderClientNameIsExist = 20117,

        /// <summary>
        /// 项目管理员不允许删除
        /// </summary>
        [DescriptionAttribute("项目管理员不允许删除")]
        ProjectAdminCannotDelete = 20117,
    }
}
