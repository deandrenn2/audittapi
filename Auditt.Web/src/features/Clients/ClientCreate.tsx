import { useRef } from "react";
import { useClient } from "../Clients/useClient";
import { ClientModel } from "./ClientModel";

type formData = {
    name?: string;
    abbreviation?: string;
    nit?: string;
    city?: string;
};

export const ClientCreate = () => {
    const { createClient } = useClient();
    const refForm = useRef<HTMLFormElement>(null);

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        const form = e.target as HTMLFormElement;

        const formData = new FormData(form);
        const client = Object.fromEntries(formData.entries()) as formData;
        const newClient: ClientModel = {
            name: client.name ?? "",
            abbreviation: client.abbreviation ?? "",
            nit: client.nit ?? "",
            city: client.city ?? "",
        };

        const response = await createClient.mutateAsync(newClient);

        if (response.isSuccess) {
            refForm.current?.reset();
        }

    }

    return (
        <div>
            <form ref={refForm} className="space-y-4" onSubmit={handleSubmit}>
                <div>
                    <label className="block text-sm font-medium mb-1">Razón Social</label>
                    <input
                    type="text"
                    name="name" 
                    required 
                    className="w-full border border-gray-300 rounded px-3 py-2 transition duration-200 hover:border-indigo-500
                     hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400" />
                </div>
                <div>
                    <label className="block text-sm font-medium mb-1">Abreviatura</label>
                    <input 
                    type="text" 
                    name="abbreviation" 
                    required 
                    className="w-full border border-gray-300 rounded px-3 py-2 transition duration-200 hover:border-indigo-500
                    hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400" />
                </div>
                <div>
                    <label className="block text-sm font-medium mb-1">NIT</label>
                    <input 
                    type="text" 
                    name="nit" 
                    required 
                    className="w-full border border-gray-300 rounded px-3 py-2 transition duration-200 hover:border-indigo-500
                    hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400" />
                </div>
                <div>
                    <label className="block text-sm font-medium mb-1">Ciudad</label>
                    <input 
                    type="text" 
                    name="city" 
                    className="w-full border border-gray-300 rounded px-3 py-2 transition duration-200 hover:border-indigo-500
                    hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400" />
                </div>
                <div>
                    <button type="submit" className=" bg-[#392F5A] hover:bg-indigo-900 text-white px-8 py-2 rounded-lg font-semibold">
                        {createClient.isPending ? "Creando..." : "Crear"}
                    </button>
                </div>
            </form>
        </div>
    );
}