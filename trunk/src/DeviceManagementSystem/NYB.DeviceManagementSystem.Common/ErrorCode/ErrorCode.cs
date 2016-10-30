using System;

namespace NYB.DeviceManagementSystem.Common
{
	/// <summary>
	/// 记录所有Web服务的错误信息
	/// 示例：10123
	/// 1系统错误；2为服务级错误
	/// 01服务模块代码
	///		01 安全管理
	///		02 采购管理
	///		03 合同管理
	///		04 系统管理
	///		05 支付管理
	///		06 甲供管理
	///		07 连续墙
	///		08 变更管理
	///		09 待处理
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

		/// <summary>
		/// 经度无效
		/// </summary>
		[Description("经度无效")]
		LongitudeError = 20010,

		/// <summary>
		/// 纬度无效
		/// </summary>
		[Description("纬度无效")]
		LatitudeError = 20011,

		/// <summary>
		/// 角度无效
		/// </summary>
		[Description("角度无效")]
		AngleError = 20012,

		/// <summary>
		/// 开始时间大于结束时间
		/// </summary>
		[Description("开始时间大于结束时间")]
		StartGreaterThanEnd = 20013,

		/// <summary>
		/// 组织不存在
		/// </summary>
		[Description("组织不存在")]
		OrganizationNoExist = 20014,

		/// <summary>
		/// 用户没有任何组织权限
		/// </summary>
		[Description("用户没有任何组织权限")]
		UserNoOrgPermission = 20015,

		/// <summary>
		/// 没有权限
		/// </summary>
		[Description("没有权限")]
		NoPermission = 20016,

		/// <summary>
		/// 发送短信失败
		/// </summary>
		[Description("发送短信失败")]
		SendSmsFailed = 20017,

		/// <summary>
		/// 名称重复
		/// </summary>
		[DescriptionAttribute("名称重复")]
		NameRepeat = 20018,

		/// <summary>
		/// 父级数据不存在
		/// </summary>
		[DescriptionAttribute("父级数据不存在")]
		NoParentData = 20019,

		/// <summary>
		/// 编号重复
		/// </summary>
		[DescriptionAttribute("编号重复")]
		NumberRepeat = 20020,

		/// <summary>
		/// 用户手机号码错误
		/// </summary>
		[DescriptionAttribute("用户手机号码错误")]
		UserPhoneError = 20021,

		/// <summary>
		/// 用户手机号码为空
		/// </summary>
		[DescriptionAttribute("用户手机号码为空")]
		UserPhoneEmpty = 20022,

		/// <summary>
		/// 生成文件文件错误
		/// </summary>
		[DescriptionAttribute("生成文件文件错误")]
		GenerateFileError = 20023,

		/// <summary>
		/// 周报不存在
		/// </summary>
		[DescriptionAttribute("周报不存在")]
		ReportWeedNoExited = 20024,

		/// <summary>
		/// 创建文件目录失败
		/// </summary>
		[DescriptionAttribute("创建文件目录失败")]
		CreateDirectoryError = 20025,

		/// <summary>
		/// 删除临时文件失败
		/// </summary>
		[DescriptionAttribute("删除临时文件失败")]
		DelTempFileError = 20026,

		/// <summary>
		/// 同一经纬度的模型显示时间有重复
		/// </summary>
		[Description("同一经纬度的模型显示时间有重复")]
		DetectionModelRepeat = 20101,

		/// <summary>
		/// 材料已退场
		/// </summary>
		[Description("材料已退场")]
		MaterialIsExited = 20201,

		/// <summary>
		/// 合同参与方重复
		/// </summary>
		[Description("合同参与方重复")]
		ContractPartiesRepeat = 20301,

		/// <summary>
		/// 支付条款内支付金额总和，大于该合同签订金额
		/// </summary>
		[Description("支付条款内支付金额总和，大于该合同签订金额")]
		TotalPaymentAmountGreaterThanTotalAmount = 20302,

		/// <summary>
		/// 支付金额，大于该合同签订金额
		/// </summary>
		[Description("支付金额，大于该合同签订金额")]
		PaymentAmountGreaterThanTotalAmount = 20303,

		/// <summary>
		/// 合同文件夹名称重复
		/// </summary>
		[Description("合同文件夹名称重复")]
		ContractTypeNameRepeat = 20304,

		/// <summary>
		/// 该合同文件夹下存在合同，不能删除
		/// </summary>
		[Description("该合同文件夹下存在合同，不能删除")]
		ContractTypeHaveContrate = 20305,

		/// <summary>
		/// 该合同文件夹下存在子级文件夹，不能删除
		/// </summary>
		[Description("该合同文件夹下存在子级文件夹，不能删除")]
		ContractTypeHaveChildren = 20306,

		/// <summary>
		/// 该合同不存在
		/// </summary>
		[Description("该合同不存在")]
		ContractNoExist = 20307,

		/// <summary>
		/// 该合同不包含此支付条款
		/// </summary>
		[Description("该合同不包含此支付条款")]
		ContractNoContainsPaymentTerm = 20308,

		/// <summary>
		/// 该合同不包含此变更
		/// </summary>
		[Description("该合同不包含此变更")]
		ContractNoContainsChange = 20309,

		/// <summary>
		/// 该合同的支付条款有关联的支付申请，不能删除
		/// </summary>
		[Description("该合同的支付条款有关联的支付申请，不能删除")]
		ContractHavePaymentApply = 20310,

		/// <summary>
		/// 该桩基类型已有材料使用记录，不能删除
		/// </summary>
		[Description("桩基类型已有材料使用记录，不能删除")]
		PileTypeHaveMaterialUsage = 20401,

		/// <summary>
		/// 该组织有子级组织，不能删除
		/// </summary>
		[Description("该组织有子级组织，不能删除")]
		OrganizationHaveChildren = 20402,

		/// <summary>
		/// 该组织下有用户，不能删除
		/// </summary>
		[Description("该组织下有用户，不能删除")]
		OrganizationHaveUser = 20403,

		/// <summary>
		/// 该组织有关联的数据，不能删除
		/// </summary>
		[Description("该组织有关联的数据，不能删除")]
		OrganizationHaveData = 20404,

		/// <summary>
		/// 该支付申请不存在
		/// </summary>
		[Description("该支付申请不存在")]
		PaymentApplyNoExist = 20501,

		/// <summary>
		/// 该类别下存在材料设备，不能删除
		/// </summary>
		[Description("该类别下存在材料设备，不能删除")]
		OwnerSupplyMaterialTypeHaveMaterial = 20601,

		/// <summary>
		/// 该类别下存在子级类别，不能删除
		/// </summary>
		[Description("该类别下存在子级类别，不能删除")]
		OwnerSupplyMaterialTypeHaveChildren = 20602,

		/// <summary>
		/// 该材料设备不存在
		/// </summary>
		[Description("该材料设备不存在")]
		OwnerSupplyMaterialNoExist = 20603,

		/// <summary>
		/// 计划单内的物料重复
		/// </summary>
		[Description("计划单内的物料重复")]
		OwnerSupplyMaterialRepeat = 20604,

		/// <summary>
		/// 该物料使用计划不存在
		/// </summary>
		[Description("该物料使用计划不存在")]
		OwnerSupplyMaterialUsagePlanNoExist = 20605,

		/// <summary>
		/// 该材料设备正在使用中，不能删除
		/// </summary>
		[Description("该材料设备正在使用中，不能删除")]
		OwnerSupplyMaterialUsage = 20606,

		/// <summary>
		/// 该连续墙不存在
		/// </summary>
		[Description("该连续墙不存在")]
		UnderWallConInfoNoExist = 20701,

		/// <summary>
		/// 连续墙编号重复
		/// </summary>
		[Description("连续墙编号重复")]
		UnderWallConInfoNumRepeat = 20702,

		/// <summary>
		/// 连续墙管路安装项目类型不存在
		/// </summary>
		[Description("连续墙管路安装项目类型不存在")]
		UUPostGroutingPipelineInstallProjectTypeNotExist = 20703,

		/// <summary>
		/// 连续墙管路安装项目类型重复
		/// </summary>
		[Description("连续墙管路安装项目类型重复")]
		UUPostGroutingPipelineInstallProjectTypeRepeat = 20704,

		/// <summary>
		/// 保存钢筋加工验收失败
		/// </summary>
		[Description("保存钢筋加工验收失败")]
		UnderWallSaveSteelProcessFailed = 20705,

		/// <summary>
		/// 成槽隐蔽工程验收不存在
		/// </summary>
		[Description("成槽隐蔽工程验收不存在")]
		UGroovingShelterCheckNoExist = 20706,

		/// <summary>
		/// 添加成槽验收失败
		/// </summary>
		[Description("添加成槽隐蔽工程验收失败")]
		UGroovingShelterCheckNoAddFiled = 20707,

		/// <summary>
		/// 添加钢筋验收加工失败
		/// </summary>
		[Description("添加钢筋验收加工失败")]
		USteelProcessNoAddFiled = 20708,

		/// <summary>
		/// 该钢筋加工验收不存在
		/// </summary>
		[Description("该钢筋加工验收不存在")]
		USteelProcessNoExist = 20709,

		/// <summary>
		/// 成槽信息不存在
		/// </summary>
		[Description("成槽信息不存在")]
		UGrooveNoExist = 20710,

		/// <summary>
		/// 保存钢筋加工失败
		/// </summary>
		[Description("保存钢筋加工失败")]
		UInstallingSteelSaveFiled = 20711,

		/// <summary>
		/// 钢筋笼验收不存在
		/// </summary>
		[Description("钢筋笼验收不存在")]
		USteelCageCheckNoExist = 20712,

		[Description("吊装添加失败")]
		USteelCageLiftingAddFiled = 20713,

		/// <summary>
		/// 连续墙后注浆管号不存在
		/// </summary>
		[Description("连续墙后注浆管号不存在")]
		UConcretePostGroutingRecordTubeNotExist = 20714,

		/// <summary>
		/// 连续墙后注浆管号重复
		/// </summary>
		[Description("连续墙后注浆管号重复")]
		UConcretePostGroutingRecordTubeRepeat = 20715,

		/// <summary>
		/// 吊装信息不存在
		/// </summary>
		[Description("吊装信息不存在")]
		USteelCageLiftingNoExist = 20716,

		/// <summary>
		/// 连续墙验收添加失败
		/// </summary>
		[Description("连续墙验收添加失败")]
		UCheckAddFiled = 20716,

		/// <summary>
		/// 连续墙验收不存在
		/// </summary>
		[Description("连续墙验收不存在")]
		UCheckNoExist = 20717,

		/// <summary>
		/// 该变更有关联的支付申请，不能删除
		/// </summary>
		[Description("该变更有关联的支付申请，不能删除")]
		ChangeHavePaymentApply = 20801,

		/// <summary>
		/// 待处理
		/// </summary>
		[Description("待处理不存在")]
		PendingNoExist = 20901,

		/// <summary>
		/// TRD编号重复
		/// </summary>
		[Description("TRD编号重复")]
		TRDNumberRepeat = 20718,

		/// <summary>
		/// TRD不存在
		/// </summary>
		[Description("TRD不存在")]
		TRDNotExist = 20719,

		/// <summary>
		/// TRD工法等厚度水泥土搅拌墙施工记录不存在
		/// </summary>
		[Description("TRD工法等厚度水泥土搅拌墙施工记录不存在")]
		TRDMehodCementConRecordNoExist = 20720,

		/// <summary>
		/// TRD水泥土搅拌墙地基工程检验批质量验收记录不存在
		/// </summary>
		[Description("TRD水泥土搅拌墙地基工程检验批质量验收记录不存在")]
		TRDFoundationProjCheckNotExist = 20721,
	}
}
