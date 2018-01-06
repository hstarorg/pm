const { db, util, crypto } = require('../common');
const { AccountSqls } = require('./sqlstore');
const { AccountSchemas } = require('./schemas');
const config = require('../config');

const _findUser = async username => {
  return await db.executeScalar(AccountSqls.FIND_USER_BY_NAME, { UserName: username });
};

const doRegister = async ctx => {
  const reqData = ctx.request.body;
  await util.validate(reqData, AccountSchemas.REGISTER_SCHEMA);
  const findUser = await _findUser(reqData.UserName);
  if (findUser) {
    util.throwError('用户名已存在，请修改');
  }
  const sqlParams = {
    UserName: reqData.UserName,
    NickName: reqData.UserName,
    Password: crypto.hmac_sha256(reqData.Password, config.hashSecret),
    IsExternalUser: 0,
    Email: '',
    Telephone: '',
    AvatarUrl: '',
    UserStatus: UserStatus.Active,
    CreateDate: Date.now()
  };
  const userId = await db.executeInsert(AccountSqls.DO_REGISTER, sqlParams);
  ctx.body = '';
};

const doLogin = async ctx => {
  const reqData = ctx.request.body;
  await util.validate(reqData, AccountSchemas.LOGIN_SCHEMA);
  if (ctx.session.captcha !== reqData.CaptchaCode.toLowerCase()) {
    util.throwError('验证码不正确');
  }
  const findUser = await _findUser(reqData.UserName);
  if (!findUser) {
    util.throwError('未找到用户');
  }
  if (findUser.UserStatus !== UserStatus.Active) {
    util.throwError('用户被禁用，请联系管理员');
  }
  if (findUser.Password !== crypto.hmac_sha256(reqData.Password, config.hashSecret)) {
    util.throwError('登录失败，账户密码不匹配');
  }
  delete findUser.Password;
  ctx.session.user = findUser;
  ctx.body = '';
};

const doLogout = async ctx => {
  ctx.session.user = null; // 清空session中的user对象
  ctx.body = '';
};

module.exports = {
  doRegister,
  doLogin,
  doLogout
};
