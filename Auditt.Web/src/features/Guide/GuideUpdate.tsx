import { useEffect, useRef, useState } from "react";
import { GuideModel } from "./GuideModel"
import { useGuide } from "./useGuide"
import { useScales } from "../Settings/Scales/useScales";
export const GuideUpdate = ({ data }: { data: GuideModel }) => {
    const { updateGuide } = useGuide();
    const [guide, setGuide] = useState<GuideModel>(data);
    const { scales, } = useScales();
    const [selectedScaleId, setSelectedScaleId] = useState<string>("");
    const refForm = useRef<HTMLFormElement>(null);

    useEffect(() => {
        if (data) {
            setGuide(data);
        }
    }, [data, setGuide])

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        const response = await updateGuide.mutateAsync(guide);

        if (response.isSuccess) {
            refForm.current?.reset();
        }
    }

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setGuide({ ...guide, [e.target.name]: e.target.value });
    }

    return (
        <div>
            <form ref={refForm} className="space-y-4" onSubmit={handleSubmit}>
                <div>
                    <label className="block text-sm font-medium mb-1">Nombre</label>
                    <input
                        type="text"
                        name="name"
                        value={guide.name}
                        required
                        className="w-full border border-gray-300 rounded px-3 py-2 transition duration-200 hover:border-indigo-500
                     hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400"
                        onChange={handleChange} />
                </div>

                <div>
                    <label className="block text-sm font-medium mb-1">Pregunta</label>
                    <input
                        type="text"
                        name="description"
                        value={guide.description}
                        required
                        className="w-full border border-gray-300 rounded px-3 py-2 transition duration-200 hover:border-indigo-500
                     hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400"
                        onChange={handleChange} />
                </div>
                <label className="block font-medium mb-2" htmlFor="idScale">Escala</label>
                
                <section className="w-full">
                    <select
                        name="idScale"
                        id="idScale"
                        className="w-full h-12 p-2 border rounded mb-4"
                        required
                        value={guide.idScale}
                        onChange={(e) => {
                            setSelectedScaleId(e.target.value); 
                            setGuide({ ...guide, idScale: parseInt(e.target.value, 10) });
                        }}
                    >
                        {scales?.map((scale) => (
                            <option key={scale.id} value={scale.id}>
                                {scale.name}
                            </option>
                        ))}
                    </select>
                </section>
                <div>
                    <button type="submit" className=" bg-indigo-500 hover:bg-indigo-900 text-white px-8 py-2 rounded-lg font-semibold">
                        {updateGuide.isPending ? "Actualizando..." : "Actualizar"}
                    </button>
                </div>
            </form>
        </div>
    )
}