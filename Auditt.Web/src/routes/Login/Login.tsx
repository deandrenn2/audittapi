
export const Login = () => {
   const apiUrl = import.meta.env.VITE_API_URL;
   const handleLogin = async (): Promise<void> => {
      window.location.href = `${apiUrl}api/auth/google-login`;
   }

   return (
      <div className="flex justify-center items-center h-screen bg-gray-200">
         <div className="flex h-screen w-full">
            <div className="w-1/2 bg-white flex flex-col justify-center items-center">
               <h1 className="text-5xl font-bold">
                  <span className="text-pink-500">Auditt</span><span className="text-gray-800">Api</span>
               </h1>
               <button
                  onClick={handleLogin}
                  className="mt-8 cursor-pointer bg-red-500 text-white text-lg font-semibold px-6 py-3 rounded-full shadow-lg hover:bg-red-700 transition duration-300">
                  Iniciar sesión con GOOGLE
               </button>
               {/* <button
                  className="mt-8 cursor-pointer bg-purple-500 text-white text-lg font-semibold px-6 py-3 rounded-full shadow-lg hover:bg-purple-700 transition duration-300">
                  Registrarse
               </button> */}
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
