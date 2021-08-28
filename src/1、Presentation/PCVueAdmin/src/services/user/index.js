import {requestPost} from '@/utils/request'
export async function newbaseuser(params)
{
     return requestPost('sys​/user​/raiseuser',params)
}
export default {
    newbaseuser
  }