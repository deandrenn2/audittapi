import { useState } from "react";
import { useFirstData, useLogin } from "../../../routes/Login/useLogin";
import { CreateUserModel } from "../../../routes/Login/LoginModel";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

export const UserCreate = () => {
   const navigate = useNavigate();
   const { queryFirstUser } = useFirstData();
   const [user, setUser] = useState<CreateUserModel>({
      firstName: '',
      lastName: '',
      email: '',
      password: '',
      securePharse: '',
   });

   const { createUser } = useLogin();

   const handleCreate = async (event: React.FormEvent<HTMLFormElement>): Promise<void> => {
      event.preventDefault();

      if (user.password !== confirmPassword) {
         toast.error('Las contraseñas no coinciden');
         return;
      }
      const res = await createUser.mutateAsync(user);

      if (!res.isSuccess) {
         toast.error(res.message);
         return;
      } else {
         queryFirstUser.refetch();
         navigate('/Create/Business');
      }
   };

   const handleChange = (event: React.ChangeEvent<HTMLInputElement>): void => {
      const { name, value } = event.target;
      event.preventDefault();
      setUser({ ...user, [name]: value });
   };

   return (
      <div className="flex w-full">
         <form onSubmit={handleCreate} className="w-full">
          
            <div>
               <label
                  htmlFor="namesTxt"
                  className="block text-gray-600 text-sm font-bold mb-2 w-full"
               >
                  Nombre
               </label>
               <div className="relative">
                  <input
                     id="namesTxt"
                     name="firstName"
                     value={user?.firstName}
                     onChange={(e) => handleChange(e)}
                     required
                     className="w-full border border-gray-300 rounded px-3 py-2 transition duration-200 hover:border-indigo-500"
                     placeholder="Nombres"
                  />
               </div>
            </div>
            <div>
               <label
                  htmlFor="lastNameTxt"
                  className="block text-gray-600 text-sm font-bold mb-2"
               >
                  Apellido
               </label>
               <div className="relative">
                  <input
                     id="lastNameTxt"
                     name="lastName"
                     value={user?.lastName}
                     onChange={(e) => handleChange(e)}
                     required
                     className="w-full px-5 py-2 border border-gray-700 rounded-md placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500"
                     placeholder="Apellidos"
                  />
               </div>
            </div>

            <div>
               <label
                  htmlFor="email"
                  className="block text-gray-600 text-sm font-bold mb-2"
               >
                  Email
               </label>
               <div className="relative">
                  <input
                     type="email"
                     id="email"
                     name="email"
                     value={user?.email}
                     onChange={(e) => handleChange(e)}
                     required
                     className="w-full px-5 py-2 border border-gray-700 rounded-md placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500 mb-2"
                     placeholder="Correo electrónico"
                  />
               </div>
            </div>
    
            <button
               type="submit"
               className="bg-indigo-500 hover:bg-indigo-900 text-white px-4 py-2 rounded-lg font-semibold">
               Crear Usuario
            </button>
         </form>
      </div>
   )
}