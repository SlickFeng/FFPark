import { ROUTES} from '@/services/api'
import {request,requestGet, METHOD, removeAuthorization} from '@/utils/request'

/**
 * 登录服务
 * @param name 账户名
 * @param password 账户密码
 * @returns {Promise<AxiosResponse<T>>}
 */
export async function login(name, password) {
  return request('auth/authenticate', METHOD.POST, {
    appid: name,
    appsecret: password
  })
}

export async function getRoutesConfig() {
  return request(ROUTES, METHOD.GET)
}

export async function getUserInfo()
{
   return requestGet('sys/user/getbaeuserinfo')
}

/**
 * 退出登录
 */
export function logout() {
  localStorage.removeItem(process.env.VUE_APP_ROUTES_KEY)
  localStorage.removeItem(process.env.VUE_APP_PERMISSIONS_KEY)
  localStorage.removeItem(process.env.VUE_APP_ROLES_KEY)
  removeAuthorization()
}
export default {
  login,
  logout,
  getRoutesConfig,
  getUserInfo
}
