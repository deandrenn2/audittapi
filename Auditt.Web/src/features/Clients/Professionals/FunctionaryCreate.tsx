import { useRef } from "react";
import { FunctionaryModel } from "./FuntionaryModel";
import { useFunctionary } from "./UseFuntionary";
type formData = {
    firstName?: string;
    lastName?: string;
    identification?: string;
};

export const FunctionaryCreate = () => {
  const { createFunctionary } = useFunctionary();
  const refForm = useRef<HTMLFormElement>(null);
  
  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const form = e.target as HTMLFormElement;
    const formData = new FormData(form);
    const functionary = Object.fromEntries(formData.entries()) as formData;
    const newFunctionary: FunctionaryModel = {
        id: 0,
        firstName: functionary.firstName ?? "",
        lastName: functionary.lastName ?? "",
        identification: functionary.identification ?? "",
        
    };
    const response = await createFunctionary.mutateAsync(newFunctionary);
        if (response.isSuccess) {
            refForm.current?.reset();
        }
}
   
    return (
        <div>
            <form ref={refForm} className="space-y-4" onSubmit={handleSubmit}>
                <div>
                    <label className="block text-sm font-medium mb-1">Nombre</label>
                    
                    <input type="text" 
                    name="firstName"
                    required
                    className="w-full border border-gray-300 rounded px-3 py-2 transition duration-200 hover:border-indigo-500
                         hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400" />
                </div>
                <div>
                    <label className="block text-sm font-medium mb-1">Apellido</label>
                    <input type="text" 
                    name="lastName"
                    required
                    className="w-full border border-gray-300 rounded px-3 py-2 transition duration-200 hover:border-indigo-500
                         hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400" />
                </div>
                <div>
                    <label className="block text-sm font-medium mb-1">Identificación</label>
                    <input type="text"
                    name="identification"
                    
                    className="w-full border border-gray-300 rounded px-3 py-2 transition duration-200 hover:border-indigo-500
                         hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400" />
                </div>
                <div>
                    <button type="submit" className="bg-[#392F5A] hover:bg-indigo-900 text-white px-8 py-2 rounded-lg font-semibold">
                        Registrar
                    </button>
                </div>
            </form>
        </div>
    );
}