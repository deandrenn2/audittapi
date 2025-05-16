import React, { useEffect, useRef, useState } from "react";
import { UserResponseModel } from "../../../routes/Login/LoginModel";
import { useUser } from "./useUser";

export const UserUpdate = ({data}: {data: UserResponseModel}) =>{
   const {updateUser} = useUser();
   const [user ,setUser] = useState<UserResponseModel>(data);
    const refForm = useRef<HTMLFormElement>(null);

    useEffect(() =>{
        if(data){
            setUser(data);
        }
    }, [data, setUser])

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        const response = await updateUser.mutateAsync(user);
        if(response.isSuccess) {
            refForm.current?.reset();
        }
    }

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setUser({ ...user, [e.target.name]: e.target.value });
    }

    return(
        <div className="flex w-full">
         <form className="w-full" onSubmit={handleSubmit}>
            <div>
               <label
                  className="block text-gray-600 text-sm font-bold mb-2 w-full">
                  Nombre
               </label>
               <div className="relative">
                  <input
                     value={user.firstName}
                     name="firstName"
                     required
                     className="w-full border border-gray-300 rounded px-3 py-2 transition duration-200 hover:border-indigo-500
                     hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400 mb-2"
                     placeholder="Nombre"
                     onChange={handleChange}
                  />
               </div>
            </div>
            <div>
               <label
                  className="block text-gray-600 text-sm font-bold mb-2">
                  Apellido
               </label>
               <div className="relative">
                  <input
                     value={user.lastName}
                     name="lastName"
                     required
                     className="w-full border border-gray-300 rounded px-3 py-2 transition duration-200 hover:border-indigo-500
                     hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400 mb-2"
                     placeholder="Apellido"
                     onChange={handleChange}
                  />
               </div>
            </div>

            <div>
               <label
                  className="block text-gray-600 text-sm font-bold mb-2">
                  Roles
               </label>
               <div className="relative">
                  <input
                     value={user.roleName}
                     name="roleName"
                     required
                     className="w-full border border-gray-300 rounded px-3 py-2 transition duration-200 hover:border-indigo-500
                     hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400 mb-2"
                     placeholder="Roles"
                     onChange={handleChange}
                  />
               </div>
            </div>

            <div>
               <label
                  className="block text-gray-600 text-sm font-bold mb-2">
                  Email
               </label>
               <div className="relative">
                  <input
                     value={user.email}
                     name="Email"
                     required
                     className="w-full border border-gray-300 rounded px-3 py-2 transition duration-200 hover:border-indigo-500
                     hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400 mb-2"
                     placeholder="Email"
                     onChange={handleChange}
                  />
               </div>
            </div>
            <button
               type="submit"
               className="bg-[#392F5A] hover:bg-indigo-900 text-white px-4 py-2 rounded-lg font-semibold cursor-pointer">
                {updateUser.isPending ? "Actualizando" : "Actualizar"}
            </button>
         </form>
      </div>
    )
}