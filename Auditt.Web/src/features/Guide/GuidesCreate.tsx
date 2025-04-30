import { useRef } from "react";
import { GuideModel } from "./GuideModel";
import { useGuide } from "./useGuide";
import { useScales } from "../Settings/Scales/useScales";


type formData = {
    name?: string;
    description?: string;
    idScale?: string;
};

export const GuidesCreate = () => {
    
    const { createGuide } = useGuide();
    const { scales,  } = useScales();
    const refForm = useRef<HTMLFormElement>(null);
    
    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        const form = e.target as HTMLFormElement;
    
        const formData = new FormData(form);
        const guide = Object.fromEntries(formData.entries()) as formData;        
        const newGuide: GuideModel = {
            name: guide.name ?? "",
            description: guide.description ?? "",
            idScale: guide.idScale ? Number(guide.idScale) : 0,
            
        };

        const response = await createGuide.mutateAsync(newGuide);

        if (response.isSuccess) {
            refForm.current?.reset();
        }
    };

    return (
        <form onSubmit={handleSubmit} ref={refForm} className="w-full">
            <label className="block font-medium mb-2" htmlFor="name">Nombre</label>
            <section className="w-full">
                <input
                    type="text"
                    name="name"
                    id="name"
                    className="w-full border border-gray-300 rounded px-3 py-2 transition duration-200 hover:border-indigo-500
                         hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400 mb-4"
                    required
                />
            </section>

            <label className="block font-medium mb-2" htmlFor="description">Descripción</label>
            <section className="w-full">
                <input
                    type="text"
                    name="description"
                    id="description"
                    className="w-full border border-gray-300 rounded px-3 py-2 transition duration-200 hover:border-indigo-500
                         hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400 mb-4"
                />
            </section>

            <label className="block font-medium mb-2" htmlFor="idScale">Escala</label>
            <section className="w-full">
                <select
                    name="idScale"
                    id="idScale"
                    className="w-full border border-gray-300 rounded px-3 py-2 transition duration-200 hover:border-indigo-500
                         hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400 mb-4"
                    required
                >
                    <option value="">Selecciona una escala</option>
                    {scales?.map((scale) => (
                        <option key={scale.id} value={scale.id}>
                            {scale.name}
                        </option>
                    ))}
                </select>
            </section>

            <button
                type="submit"
                className="bg-[#392F5A] hover:bg-indigo-800 text-white rounded-lg font-semibold px-6 py-2 "
            >
                Crear Guía
            </button>
        </form>
    );
};
