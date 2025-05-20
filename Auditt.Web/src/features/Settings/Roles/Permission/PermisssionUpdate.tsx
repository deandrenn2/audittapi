import { useEffect, useRef, useState } from "react";
import { usePermission } from "./usePermission";
import { permissionsModel } from "./PermissionModel";

export const PermisssionUpdate = ({ data }: { data: permissionsModel }) => {
    const { updatePermission } = usePermission();
    const [permissions, setPermissions] = useState<permissionsModel>(data);
    const refForm = useRef<HTMLFormElement>(null);

    useEffect(() => {
        if (data) {
            setPermissions(data);
        }
    }, [data]);

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        const response = await updatePermission.mutateAsync(permissions);
        if (response.isSuccess) {
            refForm.current?.reset();
        }
    };

    const handleChange = (
        e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
    ) => {
        setPermissions({ ...permissions, [e.target.name]: e.target.value });
    };

    return (
        <div>
            <form onSubmit={handleSubmit} ref={refForm}>
                <div className="mb-2">
                    <label className="block font-medium mb-2">Nombre</label>
                    <input
                        type="text"
                        name="name"
                        required
                        value={permissions.name}
                        onChange={handleChange}
                        className="shadow appearance-none border border-gray-300 rounded px-2 py-2 transition duration-200 hover:border-indigo-500 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400 mr-2 w-full"
                    />
                </div>
                <div className="mb-2">
                    <label className="block font-medium mb-2">Código</label>
                    <input
                        type="text"
                        name="code"
                        required
                        value={permissions.code}
                        onChange={handleChange}
                        className="shadow appearance-none border border-gray-300 rounded px-2 py-2 transition duration-200 hover:border-indigo-500 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400 mr-2 w-full"
                    />
                </div>
                <div className="mb-2">
                    <label className="block font-medium mb-2">Descripción</label>
                    <input
                        type="text"
                        name="description"
                        required
                        value={permissions.description}
                        onChange={handleChange}
                        className="shadow appearance-none border border-gray-300 rounded px-2 py-2 transition duration-200 hover:border-indigo-500 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400 mr-2 w-full"
                    />
                </div>
                <button
                    type="submit"
                    className="bg-[#392F5A] text-white px-4 py-2 rounded hover:bg-indigo-800 font-bold">
                    {updatePermission.isPending ? "Actualizando" : "Actualizar"}
                </button>
            </form>
        </div>
    );
};
