import {requestGet} from '@/utils/request' 

export async function  gettop6park()
{
  return requestGet('https://localhost:5001/api/park/GetTop6BaseParkList')
}

export async function fastnav()
{
  return requestGet('https://localhost:5001/api/park/navigations')
} 

export async function basesStatistics()
{
  return requestGet('https://localhost:5001/api/workplace/statistics')
}
export default {
    gettop6park,
    fastnav,
    basesStatistics
}