//跨域代理前缀
// const API_PROXY_PREFIX='/api'
// const BASE_URL = process.env.NODE_ENV === 'production' ? process.env.VUE_APP_API_BASE_URL : API_PROXY_PREFIX
const BASE_URL = process.env.VUE_APP_API_BASE_URL
module.exports = {
  /**
   * 登录接口
   */
  LOGIN: `${BASE_URL}auth/authenticate`,
  /**
   * 用户基本信息
   */
  USERINFO:`${BASE_URL}auth/authenticate`,
  ROUTES: `${BASE_URL}/routes`,
  GOODS: `${BASE_URL}/goods`,
  GOODS_COLUMNS: `${BASE_URL}/columns`,
}
