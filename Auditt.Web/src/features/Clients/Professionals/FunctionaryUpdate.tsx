import { useEffect, useRef, useState } from "react";
import { useFunctionary } from "./UseFuntionary";
import { FunctionaryModel } from "./FuntionaryModel";

export const FunctionaryUpdate = ({data} : {data: FunctionaryModel}) => {
    const { updateFunctionary } = useFunctionary();
    const [functionary, setFunctionary] = useState<FunctionaryModel>(data);  
    const refForm = useRef<HTMLFormElement>(null);
    
    useEffect(() => {
        if (data) {
            setFunctionary(data);
        }
    }, [data,])
   
    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        const response = await updateFunctionary.mutateAsync(functionary);

        if (response.isSuccess) {
            refForm.current?.reset();
        }
    }
   
    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {

        setFunctionary({ ...functionary, [e.target.name]: e.target.value }); 
    };
   
    return (
        <form ref={refForm} className="space-y-4" onSubmit={handleSubmit}>
        <div>
            <label className="block text-sm font-medium mb-1">Nombre</label>
            <input type="text" name="firstName" value={functionary.firstName} required className="w-full border border-gray-300 rounded px-3 py-2" onChange={handleChange} />
        </div>
        <div>
            <label className="block text-sm font-medium mb-1">Apellido</label>
            <input type="text" name="lastName" value={functionary.lastName} required className="w-full border border-gray-300 rounded px-3 py-2" onChange={handleChange} />
        </div>
        <div>
            <label className="block text-sm font-medium mb-1">Documento</label>
            <input type="text" name="identification" required value={functionary.identification} className="w-full border border-gray-300 rounded px-3 py-2" onChange={handleChange} />
        </div>
        <div>
            <button type="submit" className=" bg-indigo-500 hover:bg-indigo-900 text-white px-8 py-2 rounded-lg font-semibold">
                {updateFunctionary.isPending ? "Actualizando..." : "Actualizar"}
            </button>
        </div>
    </form>
    
    )

}