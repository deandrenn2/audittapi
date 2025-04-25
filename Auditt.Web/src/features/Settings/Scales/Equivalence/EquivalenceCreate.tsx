import { useEffect, useState } from "react";
import { EquivalenceModel } from "./EquivalenceModel";
import { useEquivalence } from "./useEquivalence";

interface Props {
    scaleId: number;
    onCreated?: () => void;
}

export const EquivalenceCreate = ({ scaleId, onCreated }: Props) => {
    const [equivalence, setEquivalence] = useState<EquivalenceModel>({
        id: 0,
        scaleId,
        name: "",
        value: 0,

    });

    const { createEquivalence } = useEquivalence();

    useEffect(() => {
        setEquivalence((prev) => ({ ...prev, scaleId }));
    }, [scaleId]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setEquivalence({
            ...equivalence,
            [e.target.name]: e.target.name === "value"
                ? parseInt(e.target.value)
                : e.target.value,
        });
    };

    const handleSubmit = async () => {
        const response = await createEquivalence.mutateAsync(equivalence);
        if (response?.isSuccess) {
            setEquivalence({ id: 0, scaleId, name: "", value: 0, });
            onCreated?.();
        }
    };

    return (
        <div>
            <div className="mb-2">
                <label>Nombre</label>
                <input
                    type="text"
                    name="name"
                    value={equivalence.name}
                    onChange={handleChange}
                    placeholder="Nombre"
                    className="w-full shadow appearance-none border border-gray-300 rounded px-2 py-2 transition duration-200 hover:border-indigo-500 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400 mr-2"
                />
            </div>
            <div className="mb-2">
                <label>Value</label>
                <input
                    type="number"
                    name="value"
                    value={equivalence.value}
                    onChange={handleChange}
                    placeholder="Valor"
                    className="w-full shadow appearance-none border border-gray-300 rounded px-2 py-2 transition duration-200 hover:border-indigo-500 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400 mr-2"
                />
            </div>
            <button
                onClick={handleSubmit}
                className="bg-indigo-600 text-white px-4 py-2 rounded hover:bg-indigo-800"
            >
                Crear Equivalencia
            </button>
        </div>
    );
};
