
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEnvelopeOpen, faEye, faEyeSlash, faLock } from '@fortawesome/free-solid-svg-icons';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useFirstData, useLogin } from './useLogin';
import useUserContext from '../../shared/context/useUserContext';
import { useBusiness } from '../Businesses/useBusiness';

export const Login = () => {
   const [email, setEmail] = useState<string>('');
   const [password, setPassword] = useState<string>('');
   const [showPassword, setShowPassword] = useState(false);

   const navigate = useNavigate();
   const { setToken, setBusiness, setUser, isAuthenticated, setIsAuthenticated } = useUserContext();
   const { hasFirstUser, hasFirstBusiness } = useFirstData();
   const { getUserMutation, logginn } = useLogin();
   const { getBusinessMutation } = useBusiness();
   const [isFetching, setIsFetching] = useState(false);

   useEffect(() => {
      if (isAuthenticated) {
         navigate('/');
      }
   }, [isAuthenticated, navigate]);

   const handleLogin = async (event: React.FormEvent<HTMLFormElement>): Promise<void> => {
      setIsFetching(true);
      event.preventDefault();
      const resLogin = await logginn.mutateAsync({ email, password });

      if (!resLogin.isSuccess) {
         setIsFetching(false);
         return;
      }
      if (resLogin.data) {
         setToken(resLogin?.data);

         const iduserCode = resLogin?.data?.claims['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']
            != undefined && !!resLogin?.data ?
            resLogin?.data?.claims['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']
            : "";

         const resUser = await getUserMutation.mutateAsync(Number(iduserCode));
         if (resUser.isSuccess) {
            setIsAuthenticated(true);
            if (resUser.data) {
               setUser(resUser.data);
            }
         }
         const resBusiness = await getBusinessMutation.mutateAsync();
         if (resBusiness.isSuccess) {
            if (resBusiness.data)
               setBusiness(resBusiness.data);
         }
      }
      setIsFetching(false);
   }



   // useEffect(() => {
   //    if (hasFirstBusiness === undefined && hasFirstUser === undefined) {
   //       return;
   //    }

   //    if (!hasFirstUser) {
   //       navigate('/Create/User');
   //       return;
   //    }

   //    if (!hasFirstBusiness) {
   //       navigate('/Create/Business');
   //       return;
   //    }
   // }, [hasFirstUser, navigate, hasFirstBusiness]);

   return (
      <div className="flex justify-center items-center h-screen bg-gray-200">
         <div className="flex h-screen w-full">
        <div className="w-1/2 bg-white flex flex-col justify-center items-center">
            <h1 className="text-5xl font-bold">
                <span className="text-pink-500">Auditt</span><span className="text-gray-800">Api</span>
            </h1>
            <button
                className="mt-8 bg-red-500 text-white text-lg font-semibold px-6 py-3 rounded-full shadow-lg hover:bg-red-700 transition duration-300">
                Iniciar sesión con GOOGLE
            </button>
        </div>

        <div className="w-1/2 relative bg-gradient-to-br from-indigo-700 to-purple-800 overflow-hidden">
                <defs>
                    <radialGradient id="grad1" cx="50%" cy="50%" r="50%">
                        <stop offset="0%" stop-color="#ffffff33" />
                        <stop offset="100%" stop-color="transparent" />
                    </radialGradient>
                </defs>
                <circle cx="400" cy="400" r="300" fill="url(#grad1)" />
                <path d="M200,300 C300,100 500,100 600,300" stroke="white" stroke-width="2" fill="none" />
                <path d="M250,400 C350,200 450,200 550,400" stroke="white" stroke-width="2" fill="none" />
                <circle cx="100" cy="700" r="5" fill="white" />
                <circle cx="700" cy="100" r="5" fill="white" />
                
        </div>
      </div>
      </div>
   );
};
