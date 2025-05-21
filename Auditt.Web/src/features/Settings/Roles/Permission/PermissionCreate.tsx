import { useRef } from "react";
import { permissionsModel } from "./PermissionModel";
import { usePermission } from "./usePermission";
type formData ={
    id?: number;
    name: string;
    code: string;
    description: string;
}
export const PermissionCreate = () => {
    const {createPermission} = usePermission();
    const refForm = useRef<HTMLFormElement>(null);

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        const form = e.target as HTMLFormElement;
        const formData =  new FormData (form);
        
        const permissions = Object.fromEntries(formData.entries()) as unknown as formData;
        const newPermissions: permissionsModel ={
            name: permissions.name ?? "",
            code: permissions.code ?? "",
            description: permissions.description ?? "",
        };

        const response = await createPermission.mutateAsync(newPermissions);
         if(response.isSuccess){
            refForm.current?.reset();
         }
         console.log("data",permissions)
    }

    return(
        <div>
        <form ref={refForm} className="space-y-4" onSubmit={handleSubmit}>
            <div>
                <label className="block text-sm font-medium mb-1">Nombre</label>
                <input
                    type="text"
                    name="name"
                    required
                    className="w-full border border-gray-300 rounded px-3 py-2 transition duration-200 hover:border-indigo-500
                 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400" />
            </div>

            <div>
                <label className="block text-sm font-medium mb-1">Code</label>
                <input
                    type="text"
                    name="code"
                    required
                    className="w-full border border-gray-300 rounded px-3 py-2 transition duration-200 hover:border-indigo-500
                 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400" />
            </div>

            <div>
                <label className="block text-sm font-medium mb-1">Descripcion</label>
                <input
                    type="text"
                    name="description"
                    required
                    className="w-full border border-gray-300 rounded px-3 py-2 transition duration-200 hover:border-indigo-500
                 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400" />
            </div>
            <div>
                <button type="submit" className=" cursor-pointer bg-[#392F5A] hover:bg-indigo-900 text-white px-8 py-2 rounded-lg font-semibold">
                    {createPermission.isPending ? "Creando..." : "Crear"}
                </button>
            </div>
        </form>
    </div>
    )

}
